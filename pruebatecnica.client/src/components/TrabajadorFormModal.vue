<script setup lang="ts">
  import { ref, watch, onMounted, computed, defineProps, defineEmits } from 'vue';
  import { Modal as BootstrapModal } from 'bootstrap';

  // --- Definición de Props y Emits ---
  const props = defineProps({
    trabajadorId: {
      type: Number,
      default: null
    }
  });
  const emit = defineEmits(['close', 'saved']);

  // --- Estado del Componente ---
  const modalElement = ref<HTMLElement | null>(null);
  let modalInstance: BootstrapModal | null = null;
  const isEditMode = computed(() => props.trabajadorId !== null);
  const isLoading = ref(true); // Carga interna del modal, empieza en true para mostrar spinner
  const isSaving = ref(false); // Estado para el botón de guardar
  const errorApi = ref<string | null>(null);
  // Añade una nueva ref para los errores de validación por campo
  const validationErrors = ref<Record<string, string[]>>({});

  const form = ref({
    id: 0,
    tipoDocumento: 'DNI',
    numeroDocumento: '',
    nombres: '',
    sexo: '',
    idDepartamento: null as number | null,
    idProvincia: null as number | null,
    idDistrito: null as number | null,
  });

  const departamentos = ref<{ id: number, nombreDepartamento: string }[]>([]);
  const provincias = ref<{ id: number, nombreProvincia: string }[]>([]);
  const distritos = ref<{ id: number, nombreDistrito: string }[]>([]);

  const isProvinciasLoading = ref(false);
  const isDistritosLoading = ref(false);

  // --- Lógica de Catálogos ---
  const fetchDepartamentos = async () => {
    const response = await fetch('/api/ubicacion/departamentos');
    if (response.ok) departamentos.value = await response.json();
  };
  const fetchProvincias = async (idDepartamento: number | null) => {
    provincias.value = [];
    distritos.value = [];
    isProvinciasLoading.value = true;
    if (idDepartamento) {
      const response = await fetch(`/api/ubicacion/provincias/${idDepartamento}`);
      if (response.ok) provincias.value = await response.json();
    }
    isProvinciasLoading.value = false;
  };
  const fetchDistritos = async (idProvincia: number | null) => {
    distritos.value = [];
    isDistritosLoading.value = true;
    if (idProvincia) {
      const response = await fetch(`/api/ubicacion/distritos/${idProvincia}`);
      if (response.ok) distritos.value = await response.json();
    }
    isDistritosLoading.value = false;
  };

  const isProvinciaDisabled = computed((): boolean => {
    return Boolean(isProvinciasLoading.value || !form.value.idDepartamento);
  });

  const isDistritoDisabled = computed((): boolean => {
    return Boolean(isDistritosLoading.value || !form.value.idProvincia);
  });

  const isSubmitDisabled = computed((): boolean => {
    return Boolean(isSaving.value || isLoading.value);
  });

  // --- Watchers para selects en cascada ---
  watch(() => form.value.idDepartamento, (newId, oldId) => {
    if (newId !== oldId) {
      form.value.idProvincia = null;
      form.value.idDistrito = null;
      fetchProvincias(newId);
    }
  });
  watch(() => form.value.idProvincia, (newId, oldId) => {
    if (newId !== oldId) {
      form.value.idDistrito = null;
      fetchDistritos(newId);
    }
  });

  // --- Lógica del Ciclo de Vida del Modal ---
  onMounted(async () => {
    // Inicialización del modal (sin cambios)
    if (modalElement.value) {
      modalInstance = new BootstrapModal(modalElement.value, { backdrop: 'static', keyboard: false });
      modalInstance.show();
      modalElement.value.addEventListener('hidden.bs.modal', () => emit('close'));
    }

    // Siempre se cargan los departamentos primero
    await fetchDepartamentos();

    // Lógica para el modo EDICIÓN
    if (isEditMode.value) {
      isLoading.value = true; // Inicia la carga

      try {
        // 1. Obtener los datos del trabajador desde la API
        const response = await fetch(`/api/trabajadores/${props.trabajadorId}`);
        if (!response.ok) throw new Error('No se pudo cargar la información del trabajador.');
        const data = await response.json();

        // 2. Asignar los campos simples al formulario
        form.value.id = data.id;
        form.value.tipoDocumento = data.tipoDocumento;
        form.value.numeroDocumento = data.numeroDocumento;
        form.value.nombres = data.nombres;
        form.value.sexo = data.sexo;

        // 3. Carga y asignación en cascada para la ubicación
        if (data.idDepartamento) {
          // Asigna el ID del departamento y ESPERA a que se carguen las provincias
          form.value.idDepartamento = data.idDepartamento;
          await fetchProvincias(data.idDepartamento);

          // Una vez cargadas las provincias, ahora sí asigna el ID de la provincia
          if (data.idProvincia) {
            form.value.idProvincia = data.idProvincia;
            await fetchDistritos(data.idProvincia);

            // Finalmente, una vez cargados los distritos, asigna el ID del distrito
            if (data.idDistrito) {
              form.value.idDistrito = data.idDistrito;
            }
          }
        }
      } catch (e: any) {
        errorApi.value = e.message;
      } finally {
        isLoading.value = false; // Finaliza la carga
      }
    } else {
      // Modo CREAR, solo terminamos la carga
      isLoading.value = false;
    }
  });


  // --- Lógica de Envío ---
  const handleSubmit = async () => {
    isSaving.value = true;
    errorApi.value = null;
    validationErrors.value = {}; // Limpia errores anteriores

    const url = isEditMode.value ? `/api/trabajadores/${props.trabajadorId}` : '/api/trabajadores';
    const method = isEditMode.value ? 'PUT' : 'POST';

    try {
      const response = await fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(form.value)
      });

      // Si la respuesta NO es OK, la procesamos como un error
      if (!response.ok) {
        // Intentamos leer el cuerpo de la respuesta como JSON
        const errorData = await response.json();

        if (response.status === 400) {
          // Si es un error de validación de modelo (varios campos)
          if (errorData.errors) {
            validationErrors.value = errorData.errors;
            errorApi.value = "Por favor, corrija los errores en el formulario.";
          }
          // Si es nuestro error personalizado de lógica de negocio (DNI duplicado)
          else if (errorData.message) {
            errorApi.value = errorData.message;
          }
          // Otro tipo de error 400
          else {
            errorApi.value = "Hubo un problema con los datos enviados.";
          }
        } else {
          // Errores 500, 404, etc.
          throw new Error(errorData.message || `Error del servidor: ${response.status}`);
        }
        return;
      }

      // Si todo fue OK
      emit('saved');
      modalInstance?.hide();

    } catch (e: any) {
      // Este catch ahora es principalmente para errores de red o JSON mal formado
      errorApi.value = "No se pudo comunicar con el servidor o la respuesta no es válida.";
      console.error(e);
    } finally {
      isSaving.value = false;
    }
  };
</script>

<template>
  <div class="modal fade" ref="modalElement" tabindex="-1">
    <div class="modal-dialog modal-lg">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title">
            <i :class="['me-2', isEditMode ? 'bi-pencil-square' : 'bi-plus-circle']"></i>
            {{ isEditMode ? 'Editar Trabajador' : 'Crear Trabajador' }}
          </h5>
          <button type="button" class="btn-close" @click="modalInstance?.hide()"></button>
        </div>
        <div class="modal-body">
          <div v-if="isLoading" class="text-center p-5">
            <div class="spinner-border text-primary" role="status" style="width: 2rem; height: 2rem;"></div>
            <p class="mt-2 text-muted">Cargando...</p>
          </div>
          <form v-else id="trabajadorForm" @submit.prevent="handleSubmit" novalidate>
            <div class="row mb-3">
              <div class="col-md-6">
                <label for="tipoDocumento" class="form-label">Tipo Documento</label>
                <select id="tipoDocumento" class="form-select" v-model="form.tipoDocumento" required>
                  <option value="DNI">DNI</option>
                  <option value="CEX">Carnet de Extranjería</option>
                </select>
              </div>
              <div class="col-md-6">
                <label for="numeroDocumento" class="form-label">Número Documento</label>
                <input type="text" id="numeroDocumento" v-model="form.numeroDocumento" required
                       :class="['form-control', { 'is-invalid': validationErrors.NumeroDocumento }]">
                <div v-if="validationErrors.NumeroDocumento" class="invalid-feedback">
                  {{ validationErrors.NumeroDocumento[0] }}
                </div>
              </div>
            </div>
            <div class="mb-3">
              <label for="nombres" class="form-label">Nombres y Apellidos</label>
              <input type="text" id="nombres" v-model="form.nombres" required
                     :class="['form-control', { 'is-invalid': validationErrors.Nombres }]">
              <div v-if="validationErrors.Nombres" class="invalid-feedback">
                {{ validationErrors.Nombres[0] }}
              </div>
            </div>
            <div class="mb-3">
              <label class="form-label">Sexo</label>
              <div :class="{ 'is-invalid': validationErrors.Sexo }">
                <div class="form-check form-check-inline">
                  <input class="form-check-input" type="radio" id="sexoM" value="M" v-model="form.sexo" required>
                  <label class="form-check-label" for="sexoM">Masculino</label>
                </div>
                <div class="form-check form-check-inline">
                  <input class="form-check-input" type="radio" id="sexoF" value="F" v-model="form.sexo">
                  <label class="form-check-label" for="sexoF">Femenino</label>
                </div>
              </div>
              <div v-if="validationErrors.Sexo" :class="['invalid-feedback', { 'd-block': validationErrors.Sexo }]">
                {{ validationErrors.Sexo[0] }}
              </div>
            </div>
            <div class="row">
              <div class="col-md-4 mb-3">
                <label for="departamento" class="form-label">Departamento</label>
                <select id="departamento" v-model="form.idDepartamento"
                        :class="['form-select', { 'is-invalid': validationErrors.IdDepartamento }]">
                  <option :value="null">-- Seleccione --</option>
                  <option v-for="d in departamentos" :key="d.id" :value="d.id">{{ d.nombreDepartamento }}</option>
                </select>
                <div v-if="validationErrors.IdDepartamento" class="invalid-feedback">
                  {{ validationErrors.IdDepartamento[0] }}
                </div>
              </div>
              <div class="col-md-4 mb-3">
                <label for="provincia" class="form-label">Provincia</label>
                <select id="provincia" v-model="form.idProvincia"
                        :class="['form-select', { 'is-invalid': validationErrors.IdProvincia }]"

                        :disabled="isProvinciaDisabled">
                  <option v-if="isProvinciasLoading" :value="null">Cargando...</option>
                  <option v-else :value="null">-- Seleccione --</option>
                  <option v-for="p in provincias" :key="p.id" :value="p.id">{{ p.nombreProvincia }}</option>
                </select>
                <div v-if="validationErrors.IdProvincia" class="invalid-feedback">
                  {{ validationErrors.IdProvincia[0] }}
                </div>
              </div>
              <div class="col-md-4 mb-3">
                <label for="distrito" class="form-label">Distrito</label>
                <select id="distrito" v-model="form.idDistrito"
                        :class="['form-select', { 'is-invalid': validationErrors.IdDistrito }]"
                        :disabled="isDistritoDisabled">
                  <option v-if="isDistritosLoading" :value="null">Cargando...</option>
                  <option v-else :value="null">-- Seleccione --</option>
                  <option v-for="di in distritos" :key="di.id" :value="di.id">{{ di.nombreDistrito }}</option>
                </select>
                <div v-if="validationErrors.IdDistrito" class="invalid-feedback">
                  {{ validationErrors.IdDistrito[0] }}
                </div>
              </div>
            </div>
            <div v-if="errorApi" class="alert alert-danger mt-3">{{ errorApi }}</div>
          </form>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="modalInstance?.hide()">Cancelar</button>
          <button type="submit" form="trabajadorForm"
                  :class="['btn', 'btn-primary']"
                  :disabled="isSubmitDisabled">
            <span v-if="isSaving" class="spinner-border spinner-border-sm me-2" role="status"></span>
            {{ isSaving ? 'Guardando...' : 'Guardar Cambios' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

<script setup lang="ts">
  import { ref, onMounted, computed, defineProps, defineEmits } from 'vue';
  import { Modal as BootstrapModal } from 'bootstrap';
  import { useUbicacion } from '@/composables/useUbicacion';

  const props = defineProps({
    trabajadorId: {
      type: Number,
      default: null
    }
  });
  const emit = defineEmits(['close', 'saved']);

  const modalElement = ref<HTMLElement | null>(null);
  let modalInstance: BootstrapModal | null = null;
  const isEditMode = computed(() => props.trabajadorId !== null);
  const isLoading = ref(true);
  const isSaving = ref(false);
  const errorApi = ref<string | null>(null);
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

  const {
    departamentos,
    provincias,
    distritos,
    isProvinciasLoading,
    isDistritosLoading,
    fetchDepartamentos,
    fetchProvincias,
    fetchDistritos
  } = useUbicacion(form);

  onMounted(async () => {
    if (modalElement.value) {
      modalInstance = new BootstrapModal(modalElement.value, { backdrop: 'static', keyboard: false });
      modalInstance.show();
      modalElement.value.addEventListener('hidden.bs.modal', () => emit('close'));
    }

    await fetchDepartamentos();

    if (isEditMode.value) {
      try {
        const response = await fetch(`/api/trabajadores/${props.trabajadorId}`);
        if (!response.ok) throw new Error('No se pudo cargar la información del trabajador.');

        const data = await response.json();
        if (data.idDepartamento) await fetchProvincias(data.idDepartamento);
        if (data.idProvincia) await fetchDistritos(data.idProvincia);

        form.value = data;

      } catch (e: any) {
        errorApi.value = e.message;
      }
    }

    isLoading.value = false;
  });

  // Lógica de Envío
  const handleSubmit = async () => {
    isSaving.value = true;
    errorApi.value = null;
    validationErrors.value = {};

    let isValid = true;
    if (!form.value.nombres) { validationErrors.value.Nombres = ['El nombre es obligatorio.']; isValid = false; }
    if (!form.value.numeroDocumento) { validationErrors.value.NumeroDocumento = ['El número de documento es obligatorio.']; isValid = false; }
    if (!form.value.sexo) { validationErrors.value.Sexo = ['Debe seleccionar un sexo.']; isValid = false; }
    if (!form.value.idDepartamento) { validationErrors.value.IdDepartamento = ['Debe seleccionar un departamento.']; isValid = false; }
    if (!form.value.idProvincia) { validationErrors.value.IdProvincia = ['Debe seleccionar una provincia.']; isValid = false; }
    if (!form.value.idDistrito) { validationErrors.value.IdDistrito = ['Debe seleccionar un distrito.']; isValid = false; }
    if (!isValid) { errorApi.value = "Por favor, corrija los errores indicados."; isSaving.value = false; return; }

    const url = isEditMode.value ? `/api/trabajadores/${props.trabajadorId}` : '/api/trabajadores';
    const method = isEditMode.value ? 'PUT' : 'POST';

    try {
      const response = await fetch(url, {
        method,
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify(form.value)
      });

      if (!response.ok) {
        const errorData = await response.json();
        if (response.status === 400 && errorData.errors) {
          validationErrors.value = errorData.errors;
          errorApi.value = "Por favor, corrija los errores en el formulario.";
        } else if (errorData.message) {
          errorApi.value = errorData.message;
        } else {
          errorApi.value = "Hubo un problema con los datos enviados.";
        }
        return;
      }

      emit('saved');
      modalInstance?.hide();

    } catch (e: any) {
      errorApi.value = "No se pudo comunicar con el servidor o la respuesta no es válida.";
      console.error(e);
    } finally {
      isSaving.value = false;
    }
  };

  const isProvinciaDisabled = computed(() => isProvinciasLoading.value || !form.value.idDepartamento);
  const isDistritoDisabled = computed(() => isDistritosLoading.value || !form.value.idProvincia);
  const isSubmitDisabled = computed(() => isSaving.value || isLoading.value);

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
                  class="btn btn-primary"
                  :disabled="isSubmitDisabled">
            <span v-if="isSaving" class="spinner-border spinner-border-sm me-2" role="status"></span>
            {{ isSaving ? 'Guardando...' : 'Guardar Cambios' }}
          </button>
        </div>
      </div>
    </div>
  </div>
</template>

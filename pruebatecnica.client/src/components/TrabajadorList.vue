<script setup lang="ts">
  import { ref, onMounted, watch } from 'vue';
  import type { Trabajador } from '@/types/trabajador';
  import TrabajadorFormModal from './TrabajadorFormModal.vue';
  import ConfirmacionModal from './ConfirmacionModal.vue';

  // ESTADO PARA LOS MODALES
  const showFormModal = ref(false);
  const showConfirmModal = ref(false);
  const selectedTrabajadorId = ref<number | null>(null);
  const trabajadorAEliminar = ref<Trabajador | null>(null);

  // LÓGICA PARA MANEJAR MODALES
  const openCreateModal = () => {
    selectedTrabajadorId.value = null;
    showFormModal.value = true;
  };
  const openEditModal = (id: number) => {
    selectedTrabajadorId.value = id;
    showFormModal.value = true;
  };
  const openDeleteConfirmModal = (trabajador: Trabajador) => {
    trabajadorAEliminar.value = trabajador;
    showConfirmModal.value = true;
  };
  const handleCloseModals = () => {
    showFormModal.value = false;
    showConfirmModal.value = false;
    trabajadorAEliminar.value = null;
  };
  const handleSaved = () => {
    handleCloseModals();
    fetchTrabajadores();
  };
  const handleConfirmDelete = async () => {
    if (!trabajadorAEliminar.value) return;
    try {
      const response = await fetch(`/api/trabajadores/${trabajadorAEliminar.value.id}`, {
        method: 'DELETE',
      });
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || 'No se pudo eliminar el trabajador.');
      }
      handleCloseModals();
      fetchTrabajadores();
    } catch (e: any) {
      error.value = e.message;
      handleCloseModals();
    }
  };

  // ESTADO Y LÓGICA DE LA LISTA Y FILTRO
  const trabajadores = ref<Trabajador[]>([]);
  const isLoading = ref<boolean>(true);
  const error = ref<string | null>(null);
  const filtroSexo = ref('');

  // Función de carga de datos actualizada para incluir el filtro
  const fetchTrabajadores = async () => {
    isLoading.value = true;
    error.value = null;
    try {
      let url = '/api/trabajadores';
      // Si hay un filtro seleccionado (no es una cadena vacía), lo añadimos a la URL
      if (filtroSexo.value) {
        url += `?sexo=${filtroSexo.value}`;
      }

      const response = await fetch(url);
      if (!response.ok) {
        const errorText = await response.text();
        throw new Error(errorText || `Error del servidor: ${response.status}`);
      }
      const data = await response.json();
      trabajadores.value = data;
    } catch (e: any) {
      error.value = e.message;
      console.error(e);
    } finally {
      isLoading.value = false;
    }
  };

  // WATCHER: Se ejecuta cada vez que el valor de 'filtroSexo' cambia
  watch(filtroSexo, () => {
    fetchTrabajadores();
  });

  const getRowClass = (sexo: string) => {
    if (sexo === 'M') return 'table-primary';
    if (sexo === 'F') return 'table-warning';
    return '';
  };

  onMounted(() => {
    fetchTrabajadores();
  });
</script>

<template>
  <div class="container mt-4 mb-5">
    <div class="card shadow-sm">
      <div class="card-header bg-light">
        <div class="d-flex justify-content-between align-items-center">
          <h4 class="mb-0 text-primary">
            <i class="bi bi-people-fill me-2"></i>
            Gestión de Trabajadores
          </h4>
          <button class="btn btn-success" @click="openCreateModal">
            <i class="bi bi-plus-circle me-2"></i>Nuevo Registro
          </button>
        </div>
      </div>
      <div class="card-body">

        <!-- SECCIÓN DE FILTROS AÑADIDA -->
        <div class="row mb-4">
          <div class="col-md-4">
            <label for="filtroSexo" class="form-label fw-bold">Filtrar por Sexo</label>
            <select id="filtroSexo" class="form-select" v-model="filtroSexo">
              <option value="">Todos</option>
              <option value="M">Masculino</option>
              <option value="F">Femenino</option>
            </select>
          </div>
        </div>
        <!-- FIN DE LA SECCIÓN DE FILTROS -->

        <div v-if="isLoading" class="d-flex justify-content-center align-items-center my-5">
          <div class="spinner-border text-primary" role="status" style="width: 3rem; height: 3rem;"></div>
          <span class="ms-3 fs-5 text-muted">Cargando trabajadores...</span>
        </div>
        <div v-if="error" class="alert alert-danger d-flex align-items-center" role="alert">
          <i class="bi bi-exclamation-triangle-fill me-2"></i>
          <div><strong>Error:</strong> {{ error }}</div>
        </div>
        <div v-if="!isLoading && !error">
          <div v-if="trabajadores.length > 0" class="table-responsive">
            <table class="table table-hover align-middle">
              <thead class="table-light">
                <tr>
                  <th>Tipo Doc.</th>
                  <th>Nro Documento</th>
                  <th>Nombres</th>
                  <th class="text-center">Sexo</th>
                  <th>Departamento</th>
                  <th>Provincia</th>
                  <th>Distrito</th>
                  <th class="text-center">Acciones</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="trabajador in trabajadores" :key="trabajador.id" :class="getRowClass(trabajador.sexo)">
                  <td><span class="badge text-bg-secondary">{{ trabajador.tipoDocumento }}</span></td>
                  <td>{{ trabajador.numeroDocumento }}</td>
                  <td>{{ trabajador.nombres }}</td>
                  <td class="text-center fw-bold">{{ trabajador.sexo }}</td>
                  <td>{{ trabajador.nombreDepartamento }}</td>
                  <td>{{ trabajador.nombreProvincia }}</td>
                  <td>{{ trabajador.nombreDistrito }}</td>
                  <td class="text-center">
                    <button class="btn btn-sm btn-outline-primary me-1" title="Editar Trabajador" @click="openEditModal(trabajador.id)">
                      <i class="bi bi-pencil-square"></i>
                    </button>
                    <button class="btn btn-sm btn-outline-danger" title="Eliminar Trabajador" @click="openDeleteConfirmModal(trabajador)">
                      <i class="bi bi-trash3"></i>
                    </button>
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <div v-else class="text-center p-5 bg-light rounded-3">
            <i class="bi bi-funnel-fill" style="font-size: 3rem; color: #6c757d;"></i>
            <h5 class="mt-3">No se encontraron trabajadores</h5>
            <p class="text-muted">No hay registros que coincidan con los filtros seleccionados.</p>
          </div>
        </div>
      </div>
    </div>
  </div>

  <TrabajadorFormModal v-if="showFormModal"
                       :trabajador-id="selectedTrabajadorId"
                       @close="handleCloseModals"
                       @saved="handleSaved" />

  <ConfirmacionModal v-if="showConfirmModal"
                     title="Confirmar Eliminación"
                     :message="`¿Está seguro de que desea eliminar a ${trabajadorAEliminar?.nombres}?`"
                     confirm-button-text="Sí, Eliminar"
                     @close="handleCloseModals"
                     @confirm="handleConfirmDelete" />
</template>

<style scoped>
  .card {
    border: none;
  }

  .card-header {
    border-bottom: 1px solid #dee2e6;
  }
</style>

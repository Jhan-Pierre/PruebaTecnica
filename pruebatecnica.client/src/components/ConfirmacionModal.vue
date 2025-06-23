<script setup lang="ts">
  import { ref, onMounted, defineProps, defineEmits } from 'vue';
  import { Modal } from 'bootstrap';

  const props = defineProps({
    title: { type: String, default: 'Confirmar Acción' },
    message: { type: String, default: '¿Está seguro de que desea realizar esta acción?' },
    confirmButtonText: { type: String, default: 'Confirmar' },
    cancelButtonText: { type: String, default: 'Cancelar' },
    confirmButtonClass: { type: String, default: 'btn-danger' }
  });

  const emit = defineEmits(['confirm', 'close']);

  const modalElement = ref<HTMLElement | null>(null);
  let modalInstance: Modal | null = null;

  onMounted(() => {
    if (modalElement.value) {
      modalInstance = new Modal(modalElement.value);
      modalInstance.show();
      modalElement.value.addEventListener('hidden.bs.modal', () => {
        emit('close');
      });
    }
  });

  const handleConfirm = () => {
    emit('confirm');
    modalInstance?.hide();
  };

  const handleClose = () => {
    modalInstance?.hide();
  };
</script>

<template>
  <div class="modal fade" ref="modalElement" tabindex="-1" aria-labelledby="confirmModalLabel" aria-hidden="true">
    <div class="modal-dialog">
      <div class="modal-content">
        <div class="modal-header">
          <h5 class="modal-title" id="confirmModalLabel">
            <i class="bi bi-exclamation-triangle-fill text-warning me-2"></i>
            {{ title }}
          </h5>
          <button type="button" class="btn-close" @click="handleClose" aria-label="Close"></button>
        </div>
        <div class="modal-body">
          <p>{{ message }}</p>
        </div>
        <div class="modal-footer">
          <button type="button" class="btn btn-secondary" @click="handleClose">{{ cancelButtonText }}</button>
          <button type="button" :class="['btn', confirmButtonClass]" @click="handleConfirm"> {{ confirmButtonText }} </button>
        </div>
      </div>
    </div>
  </div>
</template>

import { ref, watch } from 'vue';
import type { Ref } from 'vue';

interface UbicacionForm {
  idDepartamento: number | null;
  idProvincia: number | null;
  idDistrito: number | null;
}

export function useUbicacion(form: Ref<UbicacionForm>) {
  const departamentos = ref<{ id: number, nombreDepartamento: string }[]>([]);
  const provincias = ref<{ id: number, nombreProvincia: string }[]>([]);
  const distritos = ref<{ id: number, nombreDistrito: string }[]>([]);

  const isProvinciasLoading = ref(false);
  const isDistritosLoading = ref(false);

  const fetchDepartamentos = async () => {
    try {
      const response = await fetch('/api/ubicacion/departamentos');
      if (response.ok) departamentos.value = await response.json();
    } catch (error) {
      console.error("Error al cargar departamentos:", error);
    }
  };

  const fetchProvincias = async (idDepartamento: number | null) => {
    provincias.value = [];
    distritos.value = [];
    isProvinciasLoading.value = true;
    if (idDepartamento) {
      try {
        const response = await fetch(`/api/ubicacion/provincias/${idDepartamento}`);
        if (response.ok) provincias.value = await response.json();
      } catch (error) {
        console.error("Error al cargar provincias:", error);
      }
    }
    isProvinciasLoading.value = false;
  };

  const fetchDistritos = async (idProvincia: number | null) => {
    distritos.value = [];
    isDistritosLoading.value = true;
    if (idProvincia) {
      try {
        const response = await fetch(`/api/ubicacion/distritos/${idProvincia}`);
        if (response.ok) distritos.value = await response.json();
      } catch (error) {
        console.error("Error al cargar distritos:", error);
      }
    }
    isDistritosLoading.value = false;
  };

  // WATCHERS
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

  return {
    departamentos,
    provincias,
    distritos,
    isProvinciasLoading,
    isDistritosLoading,
    fetchDepartamentos,
    fetchProvincias,
    fetchDistritos
  };
}

<template>
  <Transition name="slide">
    <div
      v-if="message"
      class="flex items-center gap-3 rounded-lg border border-red-200 bg-red-50 px-4 py-3"
    >
      <i class="pi pi-exclamation-triangle text-red-500" />
      <p class="flex-1 text-sm text-red-700">
        {{ message }}
      </p>
      <button
        class="text-red-400 hover:text-red-600"
        @click="$emit('dismiss')"
      >
        <i class="pi pi-times" />
      </button>
    </div>
  </Transition>
</template>

<script setup lang="ts">
defineProps<{
  message: string | null
}>()

defineEmits<{
  dismiss: []
}>()
</script>

<style scoped>
.slide-enter-active,
.slide-leave-active {
  transition: all 0.2s;
}
.slide-enter-from,
.slide-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>

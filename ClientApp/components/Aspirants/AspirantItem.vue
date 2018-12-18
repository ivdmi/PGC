<template>
<li>
    {{item.surename}} {{item.name}} {{item.patronymic}} 
    <button v-on:click="$emit('remove', item.id)">                                  <!-- emit - передает событие наверх -->
      X
    </button>
</li>
</template>

<script>
export default {
  props: {
    item: {
      type: Object,
      required: true
    }
  }
};
</script>
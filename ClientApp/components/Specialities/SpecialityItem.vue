<template>
<li>
    <p><strong>{{item.id}} - {{item.name}}</strong></p>
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
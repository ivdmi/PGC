<template>
    <div>
<div id="people" class="col-8 offset-2">
  <v-client-table :columns="columns" :data="list" :options="options"></v-client-table>
</div>

    </div>
</template>

<script>
import axios from "axios";

export default {  
  data() {
    return {
      list: [],
      errors: [],
      columns: ["id", "name", "knowledgeId", "knowledge.name"],
      
      options: {
        headings: {
          id: "Код",
          name: "Спеціальність",
          knowledgeId: "Шифр",
          "knowledge.name": "Галузь"
          
        },
        sortable: ["id", "name"],
        filterable: ["id", "name"]
      }
    };
  },
  created: function() {
    axios
      .get(`api/specialities`)
      .then(response => {
        this.list = response.data;
      })
      .catch(e => {
        this.errors.push(e);
      });
  }
};

</script>

<style lang="scss">
.VuePagination {
  text-align: center;
}

.vue-title {
  text-align: center;
  margin-bottom: 10px;
}

.vue-pagination-ad {
  text-align: center;
}

.glyphicon.glyphicon-eye-open {
  width: 16px;
  display: block;
  margin: 0 auto;
}

th:nth-child(3) {
  text-align: center;
}

.VueTables__child-row-toggler {
  width: 16px;
  height: 16px;
  line-height: 16px;
  display: block;
  margin: auto;
  text-align: center;
}

.VueTables__child-row-toggler--closed::before {
  content: "+";
}

.VueTables__child-row-toggler--open::before {
  content: "-";
}
</style>
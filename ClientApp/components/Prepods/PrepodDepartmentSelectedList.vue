<template>
  <div>
    <vue-good-table ref="selected-table" :columns="columns" :rows="list" :sort-options="{
        enabled: true,
        initialSortBy: {field: 'acronym', type: 'asc'}
  }" row-style-class="font-14" styleClass="vgt-table condensed bordered">
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'Delete'">
          <button type="reset" class="btn btn-warning btnxs" @click="removeItem(props.row.id)" title="Видалити запис">X</button>
        </span>
        <span v-else>{{props.formattedRow[props.column.field]}}</span>
      </template>
    </vue-good-table>
  </div>
</template>

<script>
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "PrepodDepartmentSelectedList",
  components: {
    VueGoodTable
  },
  props: ["list"],

  data() {
    return {
      columns: [
        {
          label: "Кафедра",
          field: "acronym"
        },
        {
          label: "Факультет",
          field: "faculty.acronym"
        },
        {
          label: "-",
          field: "Delete",
          width: "30px"
        }
      ]
    };
  },

  methods: {
    removeItem(param) {
      // отправить event del-speciality с параметром родителю
      this.$emit("del-department", param);
    }
  }
};
</script>
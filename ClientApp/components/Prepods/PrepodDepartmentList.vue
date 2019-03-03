<template>
  <div>
    <vue-good-table
      ref="departments-table"
      :columns="columns"
      :rows="list"
      :search-options="{
            enabled: true
          }"
      row-style-class="font-14"
      styleClass="vgt-table condensed bordered"
    >
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'Add'">
          <button
            type="reset"
            class="btn btn-warning btnxs"
            @click="addItem(props.row)"
            title="Додати запис"
          >+</button>
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
  name: "PrepodDepartmentList",
  components: {
    VueGoodTable
  },

  data() {
    return {
      list: [],
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
          label: "",
          field: "Add",
          width: "30px"
        }
      ]
    };
  },

  mounted: function() {
    axios.get(`api/Prepods/departments`).then(response => {
      this.list = response.data;
    });
  },

  methods: {
    addItem(param) {
      // отправить event add-department с параметром родителю
      this.$emit("add-department", param);
    }
  }
};
</script>
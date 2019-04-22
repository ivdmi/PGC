<template>
  <div>
    <vue-good-table ref="selected-table" :columns="columns" :rows="list" row-style-class="font-14" styleClass="vgt-table condensed bordered">
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
  name: "OrderAspirantSelectedList",
  components: {
    VueGoodTable
  },
  props: ["list"],

  data() {
    return {
      columns: [
        {
          label: "Прізвище",
          field: "fio",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Курс",
          field: "course",
          type: "number",
          width: "20px",
          filterOptions: {
            enabled: true,
            placeholder: "1"
          }
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
      this.$emit("del-aspirant", param);
    }
  }
};
</script>
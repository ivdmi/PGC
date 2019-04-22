<template>
  <div>
    <vue-good-table ref="aspirants-table" :columns="columns" :rows="list" :search-options="{
            enabled: true
          }" row-style-class="font-14" styleClass="vgt-table condensed bordered">
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'Add'">
          <button type="reset" class="btn btn-warning btnxs" @click="addItem(props.row)" title="Додати запис">+</button>
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
  name: "OrderAspirantList",
  components: {
    VueGoodTable
  },
  props: ["list"],

  data() {
    return {
      // list: [],
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
          label: "",
          field: "Add",
          width: "30px"
        }
      ]
    };
  },

  // mounted: function() {
  //   axios.get(`api/Aspirants`).then(response => {
  //     this.list = response.data;
  //   });
  // },

  methods: {
    addItem(param) {
      // отправить event add-aspirant с параметром родителю
      this.$emit("add-aspirant", param);
    }
  }
};
</script>
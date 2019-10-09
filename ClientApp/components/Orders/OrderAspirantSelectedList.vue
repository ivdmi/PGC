<template>
  <div>
    <vue-good-table ref="selected-table" :columns="columns" :rows="list" row-style-class="font-14" styleClass="vgt-table condensed bordered" :select-options="{enabled: false, selectionText: 'записів додано', clearSelectionText: '' }">
      <template slot="table-row" slot-scope="props">
        <span v-if="props.column.field == 'Delete'">
          <button type="reset" class="btn btn-warning btnxs" @click="removeItem(props.row)" title="Видалити запис">X</button>
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
          label: "Спец",
          field: "specialityId",
          type: "number",
          tdClass: "td-left",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },

        {
          label: "Кафедра",
          field: "department",
          tdClass: "td-left",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },

        {
          label: "Факультет",
          field: "faculty",
          tdClass: "td-left",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },

        {
          label: "Форма",
          field: "studyForm",
          tdClass: "td-left",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },
        {
          label: "Статус",
          field: "statusType",
          tdClass: "td-left",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },

        {
          label: "Б/К",
          field: "budget",
          tdClass: "text-center",
          filterOptions: { enabled: true, placeholder: "Б" }
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
          label: "Вступ",
          field: "inputYear",
          type: "number",
          width: "20px",
          filterOptions: { enabled: true, placeholder: "2012" }
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
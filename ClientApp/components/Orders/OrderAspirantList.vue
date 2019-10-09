<template>
  <div>
    <vue-good-table ref="my-table" :columns="columns" :rows="list" :search-options="{enabled: true}" @on-selected-rows-change="selectionChanged" :select-options="{enabled: true, selectOnCheckboxOnly: true}" row-style-class="font-14" styleClass="vgt-table condensed bordered">
      <div slot="selected-row-actions" slot-scope="props">
        <button type="reset" class="btn btn-warning btnxs" @click="selectionAction(props.selectedRows)">Додати до наказу</button>
      </div>

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
    },
    selectionAction(params) {
      // скопировать отмеченные строки, вручную снять отметки (type="reset" - я отменил Submit для кнопки "Додати до наказу")
      var aspirants = this.$refs["my-table"].selectedRows.slice();
      this.$refs["my-table"].unselectAllInternal();
      // отправить event add-aspirants с параметром родителю
      this.$emit("add-aspirants", aspirants);
    }
  }
};
</script>
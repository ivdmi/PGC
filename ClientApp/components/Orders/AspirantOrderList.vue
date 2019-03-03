<template>
  <div>
    <div>
      <div>
        <vue-good-table
        ref="my-table"
        @on-selected-rows-change="selectionChanged"
          :select-options="{ enabled: true,}"          
          :columns="columns"
          :rows="list"
          :line-numbers="true"
          :sort-options="{
            enabled: true,
            initialSortBy: [            
            {field: 'course', type: 'desc'},
            {field: 'fio', type: 'asc'}            
            ]}"
          :pagination-options="{
            enabled: true,
            mode: 'pages',
            perPage: 20,
            perPageDropdown: [5, 20, 50],
            setCurrentPage: 1,
            nextLabel: '',
            prevLabel: '',
            rowsPerPageLabel: 'Строк',
            ofLabel: 'з',
            pageLabel: 'Стор.', // for 'pages' mode
            allLabel: 'All',
          }"
          row-style-class="font-14"
          styleClass="vgt-table condensed bordered"
          >
  <div slot="selected-row-actions" slot-scope="props">
    <button type="reset" class="btn btn-warning btnxs" @click="selectionAction(props.selectedRows)">Додати до наказу</button>
  </div>

  <!-- <template slot="table-row" slot-scope="props">
    <span v-if="props.column.field == 'Delete'">
      <button class="btn btn-warning btnxs" @click="removeItem(props.row.id)" title="Видалити запис">X</button>
    </span>
    <span v-else-if="props.column.field == 'Edit'">
      <button class="btn btn-warning btnxs" @click="launchAspirantEdit(props.row)" title="Редагувати запис">Зм</button>
    </span>
    <span v-else>
      {{props.formattedRow[props.column.field]}}
    </span>
  </template> -->
  </vue-good-table>  
      </div>
    </div>
  </div>
</template>

<script>
// import AspirantItem from "./AspirantItem";
// import AspirantAdd from "./AspirantAdd";
// import AspirantEdit from "./AspirantEdit";
import { eventBus } from "./../../index.js";
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "AspirantList",
  components: {
    //  AspirantItem,
    VueGoodTable,
//    AspirantAdd,
//    AspirantEdit
  },

  data() {
    return {
      list: [],
      specialities: [],
      statuses: [],
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
          label: "Спеціальність",
          field: "specialityId",
          type: "number",
          filterable: true,
          formatFn: this.formatSpecialityFn
        },
        {
          label: "Статус",
          field: "statustypeId",
          type: "number",
          formatFn: this.formatStatusFn
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
          label: "Наяв",
          field: "present",
          type: "boolean",
          width: "20px",
          tdClass: "text-center",
          formatFn: this.formatBoolFn,
          filterOptions: {
            enabled: true, // enable filter for this column
            filterDropdownItems: [
              { value: true, text: "Вчиться" },
              { value: false, text: "Ні" }
            ],
            placeholder: "Всі"
          }
        },
        {
          label: "Бюдж",
          field: "budget",
          type: "boolean",
          width: "20px",
          tdClass: "text-center",
          formatFn: this.formatBoolFn,
          filterOptions: {
            enabled: true, // enable filter for this column
            filterDropdownItems: [
              { value: true, text: "Бюджет" },
              { value: false, text: "Контракт" }
            ],
            placeholder: "Всі"
          }
        },
        {
          label: "Стац",
          field: "stationary",
          type: "boolean",
          width: "20px",
          tdClass: "text-center",
          formatFn: this.formatBoolFn,
          filterOptions: {
            enabled: true, // enable filter for this column
            filterDropdownItems: [
              { value: true, text: "Стаціонар" },
              { value: false, text: "Заочна" }
            ],
            placeholder: "Всі"
          }
        },
        {
          label: "Ден",
          field: "day",
          type: "boolean",
          width: "20px",
          tdClass: "text-center",
          formatFn: this.formatBoolFn,
          filterOptions: {
            enabled: true, // enable filter for this column
            filterDropdownItems: [
              { value: true, text: "Денна" },
              { value: false, text: "Вечірня" }
            ],
            placeholder: "Всі"
          }
        },
        {
          label: "Док",
          field: "doctorant",
          type: "boolean",
          width: "20px",
          tdClass: "text-center",
          formatFn: this.formatBoolFn,
          filterOptions: {
            enabled: true, // enable filter for this column
            filterDropdownItems: [
              { value: false, text: "Аспірант" },
              { value: true, text: "Докторант" }
            ],
            placeholder: "Всі"
          }
        },
        {
          label: "Зах",
          field: "protection",
          type: "boolean",
          width: "20px",
          tdClass: "text-center",
          formatFn: this.formatBoolFn,
          filterOptions: {
            enabled: true, // enable filter for this column
            filterDropdownItems: [
              { value: true, text: "Захистився" },
              { value: false, text: "Ні" }
            ],
            placeholder: "Всі"
          }
        },
        {
          label: "Вступ",
          field: "inputDate",
          type: "date",
          dateInputFormat: "YYYY-MM-DD",
          dateOutputFormat: "DD.MM.YYYY"
        },
        {
          label: "Захист",
          field: "protectionDate",
          type: "date",
          dateInputFormat: "YYYY-MM-DD",
          dateOutputFormat: "DD.MM.YYYY"
        },
        {
          label: "Del",
          field: "Delete"
        },
        {
          label: "Ed",
          field: "Edit"
        }
      ],



    };
  },

  mounted: function() {
    // загрузить список
    this.reloadList();

    // Спеціальності
    axios.get(`api/specialities/names`).then(response => {
      this.specialities = response.data;

      // Установить в комбобокс фильтра список специальностей
      this.$set(this.columns[3], "filterOptions", {
        enabled: true,
        filterDropdownItems: this.specialities,
        placeholder: "Всі"
      });
    });

    // Статуси
    axios.get(`api/Aspirants/statuses`).then(response => {
      this.statuses = response.data;

      // Установить в комбобокс фильтра список статусов
      this.$set(this.columns[4], "filterOptions", {
        enabled: true,
        filterDropdownItems: this.statuses,
        placeholder: "Всі"
      });
    });

  },


  methods: {
    reloadList() {
      var self = this;
      axios.get(`api/Aspirants`).then(function(response) {
        self.list = response.data;
      });
      // .catch(function(e) {
      //   self.errors.push(e)
      // });
    },

    // вызов дочерней формы
    launchAspirantAdd() {
      // скрыть список и кнопку - оставить только дочернюю форму
      this.isVisible = false;
      this.$router.push("/aspirants/aspirant-add");
    },

    addItem(item) {
      var self = this;
      axios
        .post("api/aspirants", item)
        .then(response => {
          item.id = response.data;
          // добавить в список нового аспиранта
          self.list.push(item);
          return response;
        })
        .catch(error => {
          console.log(error);
          return error;
        });
    },
    
    selectionAction(params) {
      // скопировать отмеченные строки, вручную снять отметки (type="reset" - я отменил Submit для кнопки "Додати до наказу")
      var aspirants = this.$refs["my-table"].selectedRows.slice();
      this.$refs["my-table"].unselectAllInternal();
      // отправить event add-aspirants с параметром родителю
      this.$emit('add-aspirants', aspirants)
    },

    formatBoolFn: function(value) {
      return value ? "*" : "";
    },

    formatSpecialityFn: function(value) {
      var speciality = this.specialities.find(x => x.value === value).text;
      return speciality;
    },

    formatStatusFn: function(value) {
      var status = this.statuses.find(x => x.value === value);
      return status.text;
    }
  }
};
</script>
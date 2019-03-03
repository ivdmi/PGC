<template>
  <div>
    <router-view></router-view>

    <div>
      <p>
        <button class="btn btn-warning" title="Додати запис" v-on:click="launchAdd">Додати</button>
      </p>

      <div>
        <vue-good-table
          ref="my-table"
          :columns="columns"
          :rows="list"
          :line-numbers="true"
          :sort-options="{
            enabled: true,
            initialSortBy: [            
            {field: 'course', type: 'desc'},
            {field: 'surename', type: 'asc'}            
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
            <button class="btn btn-warning btnxs" @click="selectionAction(props.selectedRows)">Дія</button>
          </div>

          <template slot="table-row" slot-scope="props">
            <span v-if="props.column.field == 'Delete'">
              <button
                class="btn btn-warning btnxs"
                @click="removeItem(props.row.id)"
                title="Видалити запис"
              >X</button>
            </span>
            <span v-else-if="props.column.field == 'Edit'">
              <button
                class="btn btn-warning btnxs"
                @click="launchEdit(props.row)"
                title="Редагувати запис"
              >Зм</button>
            </span>
            <span v-else>{{props.formattedRow[props.column.field]}}</span>
          </template>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
import AspirantAdd from "./AspirantAdd";
import AspirantEdit from "./AspirantEdit";
import axios from "axios";
import { VueGoodTable } from "vue-good-table";

export default {
  name: "AspirantList",
  components: {
    VueGoodTable,
    AspirantAdd,
    AspirantEdit
  },
  data() {
    return {
      list: [],
      // отображение связанных полей
      specialities: [],
      statuses: [],
      studyforms: [],
      departments: [],
      faculties: [],

      columns: [
        {
          label: "Прізвище",
          field: "surename",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Ім'я",
          field: "name",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Побатькові",
          field: "patronymic"
        },
        {
          label: "Спеціальність",
          field: "specialityId",
          type: "number",
          tdClass: "td-left",
          formatFn: this.formatSpecialityFn
        },
        {
          label: "Кафедра",
          field: "departmentId",
          type: "number",
          tdClass: "td-left",
          formatFn: this.formatDepartmentFn
        },

        {
          label: "Форма",
          field: "studyForm",
          type: "number",
          tdClass: "td-left",
          formatFn: this.formatStudyFormsFn
        },
        {
          label: "Статус",
          field: "statusType",
          type: "number",
          tdClass: "td-left",
          formatFn: this.formatStatusFn
        },
        {
          label: "Курс",
          field: "course",
          type: "number",
          width: "20px",
          filterOptions: { enabled: true, placeholder: " " }
        },
        { label: "Телефон", field: "phone" },
        { label: "Email", field: "email" },
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
          // sortFn: this.sortBoolFn,
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
        // {
        //   label: "Стац",
        //   field: "stationary",
        //   type: "boolean",
        //   width: "20px",
        //   tdClass: "text-center",
        //   formatFn: this.formatBoolFn,
        //   filterOptions: {
        //     enabled: true, // enable filter for this column
        //     filterDropdownItems: [
        //       { value: true, text: "Стаціонар" },
        //       { value: false, text: "Заочна" }
        //     ],
        //     placeholder: "Всі"
        //   }
        // },
        {
          label: "Докт",
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
          label: "Чол",
          field: "sex",
          type: "boolean",
          width: "20px",
          tdClass: "text-center",
          formatFn: this.formatBoolFn,
          filterOptions: {
            enabled: true, // enable filter for this column
            filterDropdownItems: [
              { value: true, text: "Чоловік" },
              { value: false, text: "Жінка" }
            ],
            placeholder: "Всі"
          }
        },
        {
          label: "Захс",
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
          label: "Народж",
          field: "birthday",
          type: "date",
          dateInputFormat: "YYYY-MM-DD",
          dateOutputFormat: "DD.MM.YYYY"
        },
        {
          label: "Вступ",
          field: "inputDate",
          type: "date",
          dateInputFormat: "YYYY-MM-DD",
          dateOutputFormat: "DD.MM.YYYY"
        },
        {
          label: "Закінчення",
          field: "graduationDate",
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
        { label: "Del", field: "Delete" },
        { label: "Ed", field: "Edit" }
      ],
      isVisible: true
    };
  },

  mounted: function() {
    // ---------- Загрузка списка ----------
    axios.get(`api/Aspirants`).then(response => {
      this.list = response.data;
    });

    // ---------- Загрузка связанных таблиц и установка комбобоксов фильтра ----------

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

    // Кафедри
    axios.get(`api/Departments/names`).then(response => {
      this.departments = response.data;
    });

    // Форма навчання
    axios.get(`api/Aspirants/studyforms`).then(response => {
      this.studyforms = response.data;
      // Установить в комбобокс фильтра список
      this.$set(this.columns[4], "filterOptions", {
        enabled: true,
        filterDropdownItems: this.studyforms,
        placeholder: "Всі"
      });
    });

    // Статуси
    axios.get(`api/Aspirants/statuses`).then(response => {
      this.statuses = response.data;
      // Установить в комбобокс фильтра список статусов
      this.$set(this.columns[5], "filterOptions", {
        enabled: true,
        filterDropdownItems: this.statuses,
        placeholder: "Всі"
      });
    });
  },

  //   // инициализировать шину событий
  //   eventBus.$on("change-aspirant", data => {
  //     if (data.action == "post") {
  //       this.addItem(data.item);
  //     }
  //     if (data.action == "put") {
  //       this.editItem(data.item);
  //     }
  //     this.isVisible = true;
  //   });
  // },

  // // разрушить шину событий, чтобы не срабатывал повторно
  // destroyed: function() {
  //   eventBus.$off();

  methods: {
    // Вызовы  дочерних  форм

    // -------------- Добавить --------------
    launchAdd() {
      this.$router.push({
        name: "AspirantAdd",
        params: {
          specialities: this.specialities,
          departments: this.departments,
          statuses: this.statuses,
          studyforms: this.studyforms
        }
      });
    },

    // -------------- Редактировать --------------
    launchEdit(item) {
      this.$router.push({
        name: "AspirantEdit",
        params: {
          item: item,
          specialities: this.specialities,
          statuses: this.statuses,
          studyforms: this.studyforms,
          departments: this.departments
        }
      });
    },

    removeItem(id) {
      var self = this;
      axios.delete("api/Aspirants/" + id).then(function(response) {
        self.list = self.list.filter(item => {
          return item.id !== id;
        });
      });
    },

    selectionAction(params) {
      var selectedRows = this.$refs["my-table"].selectedRows;
    },

    formatBoolFn: function(value) {
      return value ? "*" : "";
    },

    formatStudyFormsFn(value) {
      let sf = this.studyforms.find(x => x.value === value);
      return sf.text;
    },

    formatStudyFormsFn: function(value) {
      let sf = this.studyforms.find(x => x.value === value);
      return sf.text;
    },

    formatSpecialityFn: function(value) {
      let sp = this.specialities.find(x => x.value === value);
      return sp.text;
    },

    formatDepartmentFn: function(value) {
      let department = this.departments.find(x => x.value === value);
      return department.text;
    },

    formatStatusFn: function(value) {
      let status = this.statuses.find(x => x.value === value);
      return status.text;
    }
  }
};
</script>
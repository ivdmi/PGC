<template>
  <div>
    <router-view></router-view>

    <div>
      <div class="row">
        <div class="col-2">
          <button
            class="btn btn-warning"
            title="Додати запис"
            v-on:click="addItem"
          >
            Новий аспірант
          </button>
        </div>
        <div class="col-2">
          <label class="btn btn-warning col-12">
            Імпорт з xls
            <input
              type="file"
              multiple="false"
              id="sheetjs-input"
              accept=".xls, .xlsx"
              @change="importExcel"
              style="display:none"
            />
          </label>
        </div>
        <span class="label" id="upload-file-info"></span>

        <b-modal
          class="text-danger border-danger"
          ref="errorImportRef"
          title="Помилка імпорту з XLS файлу!"
        >
          <b-alert show variant="danger">{{ this.errorData }}</b-alert>
        </b-modal>
      </div>

      <div>
        <vue-good-table
          :columns="columns"
          :rows="list"
          :line-numbers="true"
          :sort-options="{
            enabled: true,
            initialSortBy: [
              { field: 'course', type: 'desc' },
              { field: 'surename', type: 'asc' }
            ]
          }"
          :pagination-options="{
            enabled: true,
            mode: 'pages',
            perPage: 50,
            perPageDropdown: [25, 50, 100],
            setCurrentPage: 1,
            nextLabel: '',
            prevLabel: '',
            rowsPerPageLabel: 'Строк',
            ofLabel: 'з',
            pageLabel: 'Стор.', // for 'pages' mode
            allLabel: 'All'
          }"
          row-style-class="font-14"
          styleClass="vgt-table condensed bordered"
        >
          <!-- <div slot="selected-row-actions" slot-scope="props">
            <button class="btn btn-warning btnxs" @click="selectionAction(props.selectedRows)">Дія</button>
          </div>-->
          <template slot="table-row" slot-scope="props">
            <span v-if="props.column.field == 'Delete'">
              <button
                class="btn btn-warning btnxs"
                @click="removeItem(props.row.id)"
                title="Видалити запис"
              >
                X
              </button>
            </span>
            <span v-else-if="props.column.field == 'Edit'">
              <button
                class="btn btn-warning btnxs"
                @click="editItem(props.row)"
                title="Редагувати запис"
              >
                Зм
              </button>
            </span>
            <span v-else>{{ props.formattedRow[props.column.field] }}</span>
          </template>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
// import AspirantAdd from "./AspirantAdd";
// import AspirantEdit from "./AspirantEdit";
import axios from "axios";
import { VueGoodTable } from "vue-good-table";
import XLSX from "xlsx";

export default {
  name: "AspirantList",
  components: {
    VueGoodTable,
    XLSX
    //    AspirantAdd,
    //    AspirantEdit
  },
  data() {
    return {
      list: [],
      errorData: "",
      columns: [
        {
          label: "Прізвище",
          field: "surename",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },
        {
          label: "Ім'я",
          field: "name",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },
        {
          label: "Побатькові",
          field: "patronymic"
        },
        {
          label: "Спец",
          field: "specialityId",
          type: "number",
          tdClass: "td-left",
          filterOptions: { enabled: true, placeholder: "Фільтр" }
        },
        {
          label: "Керівник",
          field: "prepod",
          tdClass: "td-left",
          width: "130px",
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
          label: "Курс",
          field: "course",
          type: "number",
          width: "20px",
          filterOptions: { enabled: true, placeholder: " " }
        },

        {
          label: "Телефон",
          field: "phone"
        },
        {
          label: "Email",
          field: "email"
        },
        {
          label: "Наяв",
          field: "present",
          width: "20px",
          tdClass: "text-center",
          filterOptions: { enabled: true, placeholder: "Є" }
        },
        {
          label: "Б/К",
          field: "budget",
          tdClass: "text-center",
          filterOptions: { enabled: true, placeholder: "Б" }
        },
        {
          label: "Д/А",
          field: "doctorant",
          tdClass: "text-center",
          filterOptions: { enabled: true, placeholder: "а" }
        },
        {
          label: "Ст",
          field: "sex",
          width: "20px",
          tdClass: "text-center",
          filterOptions: { enabled: true, placeholder: "ч" }
        },
        {
          label: "Зах",
          field: "protection",
          width: "20px",
          tdClass: "text-center",
          filterOptions: { enabled: true, placeholder: "З" }
        },
        {
          label: "Народ",
          field: "birthYear",
          type: "number",
          width: "20px",
          filterOptions: { enabled: true, placeholder: "2000" }
        },
        {
          label: "Вступ",
          field: "inputYear",
          type: "number",
          width: "20px",
          filterOptions: { enabled: true, placeholder: "2012" }
        },
        {
          label: "Закін",
          field: "graduationYear",
          type: "number",
          width: "20px",
          filterOptions: { enabled: true, placeholder: "2017" }
        },
        {
          label: "Зах",
          field: "protectionYear",
          type: "number",
          width: "20px",
          filterOptions: { enabled: true, placeholder: "2017" }
        },

        { label: "Del", field: "Delete" },
        { label: "Ed", field: "Edit" }
      ]
    };
  },

  mounted: function() {
    this.reloadList();
  },

  methods: {
    // -------------- Загрузить данные с контроллера --------------
    reloadList() {
      axios.get(`api/Aspirants`).then(response => {
        this.list = response.data;
      });
    },

    // -------------- Добавить --------------
    addItem() {
      this.$router.push("/aspirant-add");
    },

    // -------------- Редактировать --------------
    editItem(item) {
      this.$router.push({
        name: "AspirantEdit",
        params: { id: item.id }
      });
    },

    removeItem(id) {
      //      var self = this;
      //      axios.delete("api/Prepods/" + id).then(function(response) {
      //        self.list = self.list.filter(item => {
      //          return item.id !== id;
      //        });
      //      });
      axios.delete("api/Prepods/" + id).then(response => {
        this.list = this.list.filter(item => {
          return item.id !== id;
        });
      });
    },

    // -------------- Импортировать из файла Excel --------------
    importExcel: function(evt) {
      var file;
      var files = evt.target.files;

      if (!files || files.length == 0) return;

      file = files[0];

      // очистить input, чтобы можно было выбрать тот же файл повторно
      document.getElementById("sheetjs-input").value = "";

      var reader = new FileReader();
      var self = this;

      const importPromise = new Promise((resolve, reject) => {
        reader.onload = function(e) {
          // pre-process data
          var binary = "";
          var bytes = new Uint8Array(e.target.result);
          var length = bytes.byteLength;
          for (var i = 0; i < length; i++) {
            binary += String.fromCharCode(bytes[i]);
          }

          /* read workbook */
          var wb = XLSX.read(binary, { type: "binary" });

          /* grab first sheet */
          var wsname = wb.SheetNames[0];
          var ws = wb.Sheets[wsname];

          let Json = XLSX.utils.sheet_to_json(ws);

          //          axios
          //            .post(`api/prepods/import`, Json)
          //            .then(response => resolve("result"))
          //            .catch(err => {
          //              reject(new Error(err.response.data));
          //            });
        };
        reader.readAsArrayBuffer(file);
      });

      importPromise.then(
        result => this.reloadList(),
        error => {
          this.errorData = error.message;
          this.$refs.errorImportRef.show();
        }
      );
    }
  }
};
</script>
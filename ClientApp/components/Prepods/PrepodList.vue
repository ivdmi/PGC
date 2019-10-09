<template>
  <div class="col-12">
    <router-view></router-view>

    <div>
      <div class="row">
        <div class="col-2">
          <button class="btn btn-warning" title="Додати запис" v-on:click="addItem">Новий викладач</button>
        </div>
        <div class="col-2">
          <label class="btn btn-warning col-12">
            Імпорт з xls
            <input type="file" multiple="false" id="sheetjs-input" accept=".xls, .xlsx" @change="importExcel" style="display:none">
          </label>
        </div>
        <div class="col-2">
          <label class="btn btn-warning col-12">
            Імпорт з Формуляра
            <input type="file" multiple="false" id="sheetjs-formular-input" accept=".xls, .xlsx" @change="importFromFormular" style="display:none">
          </label>
        </div>
        <span class="label" id="upload-file-info"></span>

        <b-modal class="text-danger border-danger" ref="errorImportRef" title="Помилка імпорту з XLS файлу!">
          <b-alert show variant="danger">{{this.errorData}}</b-alert>
        </b-modal>
      </div>

      <div>
        <vue-good-table :columns="columns" :rows="list" :line-numbers="true" :pagination-options="{
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
          }" :search-options="{
            enabled: true
          }" row-style-class="font-14" styleClass="vgt-table condensed bordered">
          <template slot="table-row" slot-scope="props">
            <span v-if="props.column.field == 'Delete'">
              <button class="btn btn-warning btnxs" @click="removeItem(props.row.id)" title="Видалити запис">X</button>
            </span>
            <span v-else-if="props.column.field == 'Edit'">
              <button class="btn btn-warning btnxs" @click="editItem(props.row)" title="Редагувати запис">Зм</button>
            </span>
            <span v-else>{{props.formattedRow[props.column.field]}}</span>
          </template>
        </vue-good-table>
      </div>
    </div>
  </div>
</template>

<script>
import axios from "axios";
import { VueGoodTable } from "vue-good-table";
import XLSX from "xlsx";

export default {
  name: "PrepodList",
  components: {
    VueGoodTable,
    XLSX
  },
  data() {
    return {
      list: [],
      errorData: "",

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
          field: "name"
        },
        {
          label: "Побатькові",
          field: "patronymic"
        },
        {
          label: "Звання",
          field: "rank",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Ступінь",
          field: "degree",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Посада",
          field: "position",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
        },
        {
          label: "Наяв",
          field: "present",
          filterOptions: {
            enabled: true,
            placeholder: "Ф"
          }
        },
        {
          label: "Кафедри",
          field: "deparmentsString",
          filterOptions: {
            enabled: true,
            placeholder: "Фільтр"
          }
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
          label: "Народж",
          field: "birthday",
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
      ]
    };
  },

  mounted: function() {
    this.reloadList();
  },

  methods: {
    // -------------- Загрузить данные с контроллера --------------
    reloadList() {
      axios.get(`api/Prepods`).then(response => {
        this.list = response.data;
      });
    },

    // -------------- Добавить --------------
    addItem() {
      this.$router.push("/prepod-add");
    },

    // -------------- Редактировать --------------
    editItem(item) {
      this.$router.push({
        name: "PrepodEdit",
        params: { id: item.id }
      });
    },

    // -------------- Удалить --------------
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

          axios
            .post(`api/prepods/import`, Json)
            .then(response => resolve("result"))
            .catch(err => {
              reject(new Error(err.response.data));
            });
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
    },

    // -------------- Импортировать из файла Excel - Формуляра --------------
    importFromFormular: function(evt) {
      var files = evt.target.files;
      if (!files || files.length == 0) return;
      var file = files[0];

      // очистить input, чтобы можно было выбрать тот же файл повторно
      document.getElementById("sheetjs-formular-input").value = "";

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

          axios
            .post(`api/prepods/formularImport`, Json)
            .then(response => resolve("result"))
            .catch(err => {
              reject(new Error(err.response.data));
            });
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
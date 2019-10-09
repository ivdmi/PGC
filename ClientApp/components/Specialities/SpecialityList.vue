<template>
  <div>
    <vue-bootstrap4-table :rows="list" :columns="columns" :config="config" :classes="classes">
    </vue-bootstrap4-table>
  </div>
</template>

<script>
import axios from "axios";
import VueBootstrap4Table from "vue-bootstrap4-table";

export default {
  components: {
    VueBootstrap4Table
  },
  data() {
    return {
      list: [],
      columns: [
        {
          label: "Код",
          name: "id",
          filter: {
            type: "simple",
            placeholder: "id"
          },
          sort: true
        },
        {
          label: "Спеціальність",
          name: "name",
          filter: {
            type: "simple"
          },
          sort: true,
          row_text_alignment: "text-left"
        },
        {
          label: "Шифр",
          name: "knowledgeId",
          sort: true
        },
        {
          label: "Галузь",
          name: "knowledge.name",
          filter: {
            type: "simple",
            placeholder: "Галузь"
          }
        }
      ],
      config: {
        pagination: true,
        pagination_info: true,
        num_of_visibile_pagination_buttons: 20,
        checkbox_rows: true,
        highlight_row_hover: true,
        rows_selectable: true,
        multi_column_sort: false,
        card_title: "Vue Bootsrap 4 advanced table",
        selected_rows_info: true,
        per_page: 25

        //card_mode: false,
        // highlight_row_hover_color: "grey",
        // per_page_options: [5, 10, 20, 30]
      },
      classes: {
        table: "table-bordered table-striped",
        // row: "pad4",
        cell: "cellpad4"
      }
    };
  },
  created: function() {
    axios.get(`api/specialities`).then(response => {
      this.list = response.data;
    });
  }
};
</script>
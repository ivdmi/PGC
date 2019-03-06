<template>
  <div>
    <h2>Додати аспіранта</h2>
    
    <form v-on:submit.prevent="addItem">    
    <b-container fluid>
      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Прізвище:</b-col>
        <b-col cols="9">
          <b-form-input 
          size="sm" 
          type="text"
          name="Прізвище"
          v-model="aspirant.surename"
          v-validate="'required|alpha'"
          :class="{'has-error': errors.has('Прізвище')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Прізвище')" class="offset-3 alert-validate" >{{ errors.first('Прізвище') }}</div>
         
      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Ім'я:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"            
            name="Name"
            v-model="aspirant.name"
            v-validate="'required|alpha'"
            :class="{'has-error': errors.has('Name')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Name')" class="offset-3 alert-validate" >{{ errors.first('Name') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">По батькові:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"           
            name="Побатькові"
          v-model="aspirant.patronymic"
          v-validate="'alpha'"
          :class="{'has-error': errors.has('Побатькові')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Побатькові')" class="offset-3 alert-validate" >{{ errors.first('Побатькові') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Телефон:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="text"                        
            name="Телефон"
          v-model="aspirant.phone"
          v-validate="'phone'"
          :class="{'has-error': errors.has('Телефон')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Телефон')" class="offset-3 alert-validate" >{{ errors.first('Телефон') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Email:</b-col>
        <b-col cols="9">
          <b-form-input
            size="sm"
            type="email"
            name="Email"
          v-model="aspirant.email"
          v-validate="'email'"
          :class="{'has-error': errors.has('Email')}"
          ></b-form-input>
        </b-col>
      </b-row>
      <div v-if="errors.has('Email')" class="offset-3 alert-validate" >{{ errors.first('Email') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Дата народження:</b-col>
        <b-col cols="9">
          <b-form-input
            name="Birthday"
            type="date"
            v-model="aspirant.birthday"
            size="sm"            
            min="1935-01-01"
            max="2000-12-31"
            v-validate="'date_format:YYYY-MM-DD|date_between:1935-01-01,2000-12-31'"            
          :class="{'has-error': errors.has('Birthday')}"
          ></b-form-input>
        </b-col>
      </b-row>
     <div v-if="errors.has('Birthday')" class="offset-3 alert-validate" >{{ errors.first('Birthday') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Докторант:</b-col>
        <b-col cols="9">
          <span>
            <input type="checkbox" class="short-check" id="doctorant" v-model="aspirant.doctorant">
          </span>
        </b-col>
      </b-row>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Стать:</b-col>
        <b-col cols="9">
          <span>
            <input type="checkbox" class="short-check" id="doctorant" v-model="aspirant.sex">
          </span>
        </b-col>
      </b-row>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Кафедра:</b-col>
        <b-col cols="9">
          <v-select name="Кафедра" label="text" :options="departments" v-model="selectedDepartment" v-validate="'required'" :class="{'has-error': errors.has('Кафедра')}">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
      <div v-if="errors.has('Кафедра')" class="offset-3 alert-validate" >{{ errors.first('Кафедра') }}</div>

      <b-row class="pad-4">
        <b-col cols="3" class="text-right">Спеціальність:</b-col>
        <b-col cols="9">
          <v-select name="Спеціальність" label="text" :options="specialities" v-model="selectedSpeciality" v-validate="'required'" :class="{'has-error': errors.has('Спеціальність')}">
            <template slot="option" slot-scope="option">
              <span v-html="option.text"></span>
            </template>
          </v-select>
        </b-col>
      </b-row>
      <div v-if="errors.has('Спеціальність')" class="offset-3 alert-validate" >{{ errors.first('Спеціальність') }}</div>

  <div class="form-group">
    <input type="submit" class="btn btn-primary" value="Додати наказ"/>
  </div>

    </b-container>

  </form>

  
    <!-- <button ref="createBtn" class="btn btn-warning" v-on:click="addItem('post')">Create</button> -->
    <!-- <button class="btn btn-warning" v-on:click="createAspirant('cancel')">Cancel</button> -->
    <!-- <router-link class="btn btn-warning" to="/aspirants">Cancel</router-link> -->
    <hr>
  </div>
</template>

<script>
import axios from "axios";
import moment from "moment";
import vSelect from "vue-select";

export default {
  components: {
    vSelect, axios
  },
  data() {
    return {
      aspirant: {        
        name: "",
        surename: "",
        patronymic: "",

        phone: "",
        email: "",

        birthday: "",
        inputDate: "",
        protectionDate: "",

        present: true,
        budget: true,
        doctorant: false,
        sex: false,

        specialityId: 21,
        departmentId: 1,
        statusType: 1,
        studyForm: 1
      },

      selectedSpeciality: "",
      selectedDepartment: "",

      departments: [],
      specialities: [],
      prepods: []
    };
  },

  mounted: function() {
  const responseAspirant = axios.get("api/Aspirants/" + this.$route.params.id);    
    const responseLists = axios.get(
      "api/Aspirants/lists/" + this.$route.params.id
    );

    axios
      .all([responseAspirant, responseLists])
      .then(responses => {
        this.model = responses[0].data;        

        this.statuses = responses[1].data.statuses;
        this.degrees = responses[1].data.degrees;
        this.positions = responses[1].data.positions;
        this.selectedRank = responses[2].data.selectedRank;
        this.selectedDegree = responses[2].data.selectedDegree;
        this.selectedPosition = responses[2].data.selectedPosition;
      })
      .catch(err => {
        console.log(err);
      });


    // axios.get(`api/specialities/names`).then(response => {this.specialities = response.data;});

    //axios.get(`api/departments`).then(response => {this.departments = response.data;});

    this.specialities = this.$route.params.specialities;
    this.departments = this.$route.params.departments;

    // // Программное нажатие кнопки createBtn
    // window.addEventListener("keydown", e => {
    //   if (e.keyCode == 13) {
    //     const elem = self.$refs.createBtn;
    //     elem.click();
    //   }
    // });
  },

  methods: {

addItem() {
      this.$validator.validate().then(valid => {
        if (valid) {
          this.aspirant.specialityId = this.selectedSpeciality.value;
          this.aspirant.departmentId = this.selectedDepartment.value;
          axios.post("api/aspirants", this.aspirant).then(response => {            
            this.$router.push("/aspirants");
      });
    }
 

  //  addItem() {      
  //     axios
  //       .post("api/aspirants", this.aspirant)
  //       .then(response => {          
  //         this.$router.push("/aspirants");
  //       })
  //       .catch(error => {
  //         console.log(error);
  //         return error;
  //       });
  //   },


        // createAspirant: function(action) {
        //   var backparam;
        //   this.$validator.validate().then(valid => {
        //     if (valid || action == "cancel") {
        //       if (action == "post") {
        //         backparam = {
        //           item: {
        //             name: this.name,
        //             surename: this.surename,
        //             patronymic: this.patronymic,
        //             phone: this.phone,
        //             email: this.email,
        //             birthday: this.birthday,
        //             statustypeId: 1,
        //             specialityId: this.selectedSpeciality.value,
        //             doctorant: this.doctorant
        //           },
        //           action: "post"
        //         };
        //       } else {
        //         backparam = {
        //           action: "cancel"
        //         };
        //       }
        //       eventBus.$emit("change-aspirant", backparam);
        //       this.$router.push("/aspirants");
        //     }
      
      
      });
    }
  }
};
</script>
<template>
    <div id="home">
      <b-nav pills class="col-8 offset-2">        
        <b-nav-item to="/specialities">Спеціальності</b-nav-item>
        <b-nav-item to="/aspirants">Аспіранти</b-nav-item>
        <b-nav-item to="/faculties">Факультети</b-nav-item>
        <b-nav-item to="/departments">Кафедри</b-nav-item>
        <b-nav-item to="/prepods">Викладачі</b-nav-item>
        <b-nav-item to="/orders">Накази</b-nav-item>
      </b-nav>
      <router-view></router-view> 
    </div>
</template>

<script>
// import bTable from "bootstrap-vue/es/components/table/table";
export default {
  // component: "bTable",
};
</script>

<style lang="scss">
@import "./variables.scss";

#home {
  // max-width: 1700px;
  margin: 0 auto;
  padding: 0em 1em;
  line-height: 1.2;
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: $vue-blue;
}

h1 {
  text-align: center;
}
</style>

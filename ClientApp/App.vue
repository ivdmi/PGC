<template>
    <div id="home">
        <h1>Hello World!</h1>
         
         <b-alert show> Hello Bootstrap! </b-alert>


<b-nav pills>
  <b-nav-item to="/todo">Todo List</b-nav-item>
  <b-nav-item to="/specialities">Спеціальності</b-nav-item>
  <b-nav-item to="/aspirants">Аспіранти</b-nav-item>
  <b-nav-item to="/products">Products</b-nav-item>
</b-nav>

<!-- <b-card no-block>
  <b-tabs>
    <b-tab title="Tab1">
      Tab 1 Contents
    </b-tab>
    <b-tab title="Tab2">
      Tab 2 Contents
    </b-tab>
  </b-tabs>
</b-card> -->

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
  max-width: 800px;
  margin: 0 auto;
  line-height: 1.4;
  font-family: "Avenir", Helvetica, Arial, sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
  color: $vue-blue;
}

h1 {
  text-align: center;
}
</style>

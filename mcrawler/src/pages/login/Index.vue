<template>
  <el-container class="flued">
    <div class="login_box">
      <div class="box_title">Login Page</div>
      <router-link to="home">
        <el-button type="primary">Home</el-button>
      </router-link>
    </div>
  </el-container>
</template>

<script setup>
console.log("login page loadding");
</script>

<style scoped>
.flued {
  height: 100%;
  background-color: rgba(#000, #000, #000, 0.1);
  display: flex;
  align-items: center;
  justify-content: center;
}
.login_box {
  margin: 0;
  width: 400px;
  height: 250px;
  border-radius: 5px;
  box-shadow: 10px 5px 10px black;
  background-color: rgb(211, 101, 22);
}
.box_title {
  height: 50px;
  line-height: 50px;
  width: 100%;
  color: white;
  background-color: rosybrown;
  margin-bottom: 10px;
}
</style>
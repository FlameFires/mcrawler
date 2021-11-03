<template >
  <el-card shadow="always" class="box-card task-block" @click="$emit('onClick')">
    <div class="areas">
      <div class="task-img">
        <img class="image" :src="img" />
      </div>
      <div class="task-info">
        <h2>{{ taskInfo.taskName }}</h2>
        <ul>
          <li>
            <div class="task-status">
              任务状态:
              <span class="task-status-success">完成</span>
            </div>
          </li>
          <li>
            <div class="task-status">
              执行时间:
              <span class="task-status-success">{{ taskInfo.createDate }}</span>
            </div>
          </li>
        </ul>
      </div>
      <el-button type="primary" @click.stop="$emit('onUpdate')">update</el-button>
    </div>
  </el-card>
</template>

<script>
import { defineComponent, computed, ref, toRefs, reactive, toRef, inject } from "vue"
import viewModel from '../view-model/task-block'

const imgs = ['../../static/imgs/1.jpg', '../../static/imgs/2.jpg', '../../static/imgs/3.jpg', '../../static/imgs/4.png']

const { data, props, mths } = viewModel

export default defineComponent({
  name: "TaskBlock",
  props,
  setup(props, context) {



    return { data, ...mths }
  },
})
</script>

<style>
.el-card {
  margin-bottom: 15px;
  padding: 0;
  padding-bottom: 7px;
  min-height: 100px;
  max-height: 190px;
}
.areas {
  width: 100%;
  height: 100%;
  padding: 0;
}
.task-img > img {
  float: left;
  max-width: 100px;
  display: block;
  margin-right: 20px;
}

.task-info > h2 {
  font-size: 1.2rem;
  font-weight: bold;
}
.task-info > ul {
  text-align: left;
  margin-top: 15px;
}

.task-info > ul > li {
  font-size: 0.9rem;
  padding: 7px 5px;
}

.task-status-success {
  color: rgb(48, 226, 48);
}

.task-status-error {
  color: rgb(255, 0, 0);
}

.task-status-warn {
  color: rgb(235, 231, 28);
}

.task-block:hover {
  background-color: rgba(255, 255, 255, 0.9);
  box-shadow: 0px 0px 10px rgb(155, 155, 155);
}

.task-block:active {
  background-color: rgba(240, 240, 240, 0.9);
  box-shadow: 0px 0px 10px rgb(155, 155, 155) inset;
}
</style>
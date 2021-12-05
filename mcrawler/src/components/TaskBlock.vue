<template >
  <el-card shadow="hover" class="box-card task-block" @click="$emit('onClick')">
    <div class="task-info">
      <h2>{{ taskInfo.taskName }}</h2>
      <ul class="leftBox">
        <!-- <li>
            任务状态:
            <span class="task-status-success">完成</span>
        </li>-->
        <li>
          请求类型:
          <span>{{ taskInfo.method }}</span>
        </li>
        <li>
          请求链接:
          <span>{{ taskInfo.url }}</span>
        </li>
        <li>
          执行时间:
          <span>{{ taskInfo.invokeDate || '未执行' }}</span>
        </li>
        <li>
          任务描述:
          <span>{{ taskInfo.taskDescribe }}</span>
        </li>
      </ul>
      <div class="rightBox">
        <el-tooltip content="修改" placement="top">
          <el-button class="block_edit" icon="el-icon-edit" circle @click.stop="$emit('onUpdate')"></el-button>
        </el-tooltip>
      </div>
    </div>

    <!-- 删除按钮 -->
    <el-button type="text" class="close_btn" @click.stop="$emit('onDelete')">
      <i class="el-icon el-icon-close"></i>
    </el-button>

    <!-- 修改按钮 -->
    <!-- <el-button class="block_del" icon="el-icon-edit" circle @click.stop="$emit('onUpdate')"></el-button> -->
  </el-card>
</template>

<script>
import { defineComponent, computed, ref, toRefs, reactive, toRef, inject } from "vue"
import viewModel from '../view-model/task-block'

const { data, props, mths } = viewModel

export default defineComponent({
  name: "TaskBlock",
  props,
  setup(props, context) {

    return { data, ...mths }
  },
})
</script>

<style scoped>
.el-card {
  margin-bottom: 15px;
  padding: 0;
  padding-bottom: 20px;
  min-height: 100px;
  max-height: 190px;
  font-family: "Franklin Gothic Medium", "Arial Narrow", Arial,
    sans-serif "Times New Roman", Times, serif;
  position: relative;
}
.task-info > h2 {
  font-size: 1.2rem;
  font-family: "Courier New", Courier, monospace;
  font-weight: bold;
}
.task-info > ul {
  text-align: left;
  margin-top: 15px;
}

.task-info > ul > li {
  font-size: 0.9rem;
  white-space: nowrap;
  overflow: hidden;
  text-overflow: ellipsis;
  padding: 7px 5px;
}

.task-info span {
  letter-spacing: 1px;
  margin-left: 10px;
}

.leftBox {
  width: 80%;
  float: left;
  box-sizing: border-box;
}

.leftBox > li:first-of-type {
  text-transform: uppercase;
}

.rightBox {
  width: 20%;
  float: left;
  box-sizing: border-box;
}

.task-status-success {
  color: rgb(48, 226, 48);
  font-weight: 1000;
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

.block_edit {
  margin-top: 10px;
  border: 1px;
}

.close_btn {
  position: absolute;
  top: 5px;
  right: 25px;
  padding: 0;
  background: 0 0;
  border: none;
  cursor: pointer;
  outline: 0;
  color: black;
  font-size: var(--el-message-close-size, 16px);
}

.close_btn:hover {
  color: red;
}
</style>
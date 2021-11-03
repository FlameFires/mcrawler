<template>
  <task-dialog
    ref="taskDialogRef"
    :form-type="dialogType"
    :task-info="selectedTaskInfo"
    @onSuccessed="onSuccessedFunc"
  ></task-dialog>

  <el-container style="height: 100%">
    <el-container>
      <task-menu v-if="config.showMenu"></task-menu>

      <el-container style="width: 100%">
        <el-header style="text-align: left">
          <el-button type="primary">hello</el-button>
          <el-button type="primary">hello</el-button>
          <el-button type="primary">hello</el-button>

          <el-button icon="el-icon-search" size="medium" circle @click="login"></el-button>
          <el-input
            v-model="searchText"
            @keyup.enter="search"
            placeholder="Please input"
            class="input-with-select"
          >
            <template #append>
              <el-button icon="el-icon-search" @click="search"></el-button>
            </template>
          </el-input>
        </el-header>

        <el-main style="margin-top: 60px;text-align: center;">
          <!-- 任务主题 -->

          <div
            v-infinite-scroll="load"
            infinite-scroll-delay="1000"
            class="infinite-list"
            ref="taskInfoList"
          >
            <el-row :gutter="15" style="height: 72vh;">
              <el-col
                :xs="24"
                :sm="12"
                :md="12"
                :lg="8"
                :xl="8"
                :span="8"
                v-for="(TaskInfo, index) in taskInfoList"
                :key="index"
              >
                <task-block
                  @onClick="onClickFunc(index)"
                  @onUpdate="onUpdateFunc(index)"
                  :task-info="TaskInfo"
                  :img="TaskInfo.img"
                  class="task-block"
                ></task-block>
              </el-col>
              <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8" :span="8">
                <task-block-add
                  class="task-block"
                  @click="dialogType = 'create'; selectedTaskInfo = {}; taskDialogRef.open()"
                ></task-block-add>
              </el-col>
              <!-- <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8" :span="8">
                <task-block-add @click="electItem()" class="task-block"></task-block-add>
              </el-col>-->
            </el-row>

            <!-- <el-row
              :gutter="15"
              :justify="space - around"
              v-for="(row, rowIndex) in rowNum"
              :key="rowIndex"
            >
              <el-col
                :xs="24"
                :sm="12"
                :md="12"
                :lg="8"
                :xl="8"
                :span="8"
                v-for="(colItem,colIndex) in colItems"
                :key="rowIndex * 3 + colIndex"
              >
                <TaskBlock
                  @click="electItem(rowIndex * 3 + colIndex)"
                  :task-info="colItem"
                  class="task-block"
                ></TaskBlock>
              </el-col>
            </el-row>-->
            <!-- <el-col :xs="24" :sm="12" :md="12" :lg="8" :xl="8" :span="8">
              <task-block-add @click="electItem()" class="task-block"></task-block-add>
            </el-col>-->
          </div>

          <!-- <template>
            <el-pagination background layout="prev, pager, next" :total="1000"></el-pagination>
          </template>-->

          <!-- 任务主题end -->
        </el-main>
      </el-container>
    </el-container>
  </el-container>
</template>

<script>
// 导入组件
import TaskBlock from "../../components/TaskBlock.vue";
import TaskBlockAdd from "../../components/TaskBlockAdd.vue";
import TaskDialog from "../../components/TaskDialog.vue";
import TaskMenu from "../../components/TaskMenu.vue";
// 导入数据
import viewModel from "../../view-model/home.js"
// vue apis
import { onMounted, onBeforeMount, onUpdated, provide, toRefs } from 'vue'

const { data, coms, mths } = viewModel

export default {
  components: {
    TaskBlock,
    TaskBlockAdd,
    TaskDialog,
    TaskMenu
  },
  setup() {
    HookFunc()

    data.taskInfoList.forEach(item => item.img = '../../static/imgs/2.jpg')


    return {
      ...toRefs(data),
      ...mths,
      ...coms,
    }
  }
}

function HookFunc() {


  onMounted(() => {
    mths.search()
  })
}

</script>

<style scoped>
.el-header,
.el-footer {
  background-color: #92b0f0;
  color: var(--el-text-color-primary);
  text-align: center;
  line-height: 60px;
}

/* .el-aside {
  background-color: #d3dce6;
  color: var(--el-text-color-primary);
  text-align: center;
  line-height: 200px;
} */

.el-main {
  background-color: #e9eef3;
  color: var(--el-text-color-primary);
  text-align: center;
}

.infinite-list {
  height: 72vh;
  overflow-y: auto;
  overflow-x: hidden;
}
</style>
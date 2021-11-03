<template>
  <resolver-dialog ref="resolverDialogRef" :task-info="taskInfo" :res-content="resData"></resolver-dialog>
  <el-dialog v-model="visible" :append-to-body="true" :title="title" center>
    <el-form
      ref="formRef"
      :model="taskInfo"
      :status-icon="true"
      label-position="right"
      label-width="80px"
      :rules="rules"
    >
      <el-form-item label="任务名称" prop="taskName">
        <el-input @keyup.enter="onSubmit" v-model="taskInfo.taskName"></el-input>
      </el-form-item>
      <el-row :xs="24" :sm="24" :md="24" :lg="12" :xl="12">
        <el-col :span="14">
          <el-form-item label="请求链接" prop="url">
            <el-input @keyup.enter="onSubmit" v-model="taskInfo.url"></el-input>
          </el-form-item>
        </el-col>
        <el-col class="line" :span="1"></el-col>
        <el-col :span="9">
          <el-form-item label="请求类型" prop="method">
            <el-select v-model="taskInfo.method" placeholder="请输入请求类型">
              <el-option
                v-for="(item,index) in selectItems.method"
                :value="item.value"
                :label="item.value"
                :key="index"
                :checked="index === 0"
              ></el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
      <el-form-item label="头部信息" prop="header">
        <el-input
          @keyup.enter="onSubmit"
          v-model="taskInfo.header"
          :clearable="true"
          type="textarea"
          place
          :autosize="{ minRows: 4, maxRows: 6 }"
        ></el-input>
      </el-form-item>
      <el-form-item label="任务描述">
        <el-input
          :autosize="{ minRows: 4, maxRows: 6 }"
          v-model="taskInfo.taskDescribe"
          placeholder="任务描述"
          type="textarea"
        ></el-input>
      </el-form-item>
      <el-row v-show="cfg.updateEnable">
        <el-col :span="14">
          <el-form-item label="解析式" prop="resolvePattern">
            <el-input v-model="taskInfo.resolvePattern"></el-input>
          </el-form-item>
        </el-col>
        <el-col class="line" :span="1"></el-col>
        <el-col :span="9">
          <el-form-item label="解析方式" prop="resolveType">
            <el-select v-model="taskInfo.resolveType" placeholder="请选择解析方式">
              <el-option
                v-for="(item,index) in selectItems.rType"
                :value="item.value"
                :label="item.label"
                :key="index"
                :checked="index === 0"
              ></el-option>
            </el-select>
          </el-form-item>
        </el-col>
      </el-row>
    </el-form>
    <template #footer>
      <span class="dialog-footer">
        <!-- 保存按钮 -->
        <el-button
          v-show="cfg.leftButtonEnable"
          type="primary"
          @click="onSubmit('form')"
        >{{ cfg.leftButtonText }}</el-button>

        <!-- 查看个性化按钮 -->
        <el-button
          v-show="cfg.checkBtnEnable"
          :type="cfg.checkBtnType"
          @click="start"
        >{{ cfg.checkBtnText }}</el-button>
        <el-button v-show="cfg.deleteBtnEnable" type="danger" @click="dele">删除</el-button>

        <!-- 解析 -->
        <el-button type="success" v-show="cfg.resolverBtnEnalbe" @click="resolveOpen">解析</el-button>

        <!-- 关闭 -->
        <el-button v-show="cfg.rightButtonEnable" @click="onClose">{{ cfg.rightButtonText }}</el-button>
      </span>
    </template>
  </el-dialog>
</template>

<script>
import { defineComponent, ref, toRefs, watch, toRaw, onUpdated, inject, toRef, onMounted, reactive } from 'vue'
import { ElMessage, ElMessageBox } from 'element-plus'
import viewModel from '../view-model/task-dialog'
import { task } from '../http/http'
const { data, props } = viewModel

import ResolverDialog from './ResolverDialog.vue'

export default defineComponent({
  props,
  emits: ['onSuccessed', 'onSubmit'],
  components: {
    ResolverDialog
  },
  setup(props, context) {
    //#region context example 

    // Attribute (非响应式对象，等同于 $attrs)
    // console.log(context.attrs)

    // 插槽 (非响应式对象，等同于 $slots)
    // console.log(context.slots)

    // 触发事件 (方法，等同于 $emit)
    // console.log(context.emit)

    // 暴露公共 property (函数)
    // console.log(context.expose)

    // console.log(toRaw(dialogForm.value), props.dialogFormVisible)
    //#endregion

    //#region Hook Func

    onUpdated(() => {
      {
        data.cfg.leftButtonEnable = true
        data.cfg.rightButtonEnable = true
        data.cfg.checkBtnEnable = false
        data.cfg.deleteBtnEnable = false
        data.cfg.updateEnable = false
        data.cfg.resolverBtnEnalbe = false
      }
      if (data.formRef) data.formRef.clearValidate()
      const formType = toRaw(props.formType)
      if (formType === 'create') {
        data.title = '添加任务'
        data.cfg.leftButtonText = "添加"
        data.cfg.updateEnable = true
      }
      if (formType === 'update') {
        data.title = '修改任务'
        data.cfg.leftButtonText = "保存"
        data.cfg.updateEnable = true
      }
      if (formType === 'check') {
        data.title = '查看任务'
        data.cfg.leftButtonEnable = false
        data.cfg.rightButtonEnable = false
        data.cfg.checkBtnEnable = true
        data.cfg.deleteBtnEnable = true
        data.cfg.resolverBtnEnalbe = true
      }
    })
    //#endregion

    //#region Emit Func
    const onClose = e => data.visible = false

    const createHandle = postData =>
      ElMessageBox.confirm(
        '确定新增?',
        '提示',
        {
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'info',
          center: true,
        }
      ).then(() => {
        task.add(postData).then(res => {
          console.log(res);
          if (res.ok) {
            ElMessage.success('添加成功')
            context.emit('onSuccessed', res)
            setTimeout(() => {
              onClose()
            }, 1000)
          } else
            ElMessage.error(res.message)
        })
      }).catch((action) => {
        console.log(updateHandle, action);
      })

    const updateHandle = postData =>
      ElMessageBox.confirm(
        '确定修改?',
        '提示',
        {
          distinguishCancelAndClose: true,
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning',
          center: true,
        }
      ).then(() => {
        task.update(postData)
          .then(res => {
            if (res.ok) {
              ElMessage.success('修改成功')
              context.emit('onSuccessed', res)
              setTimeout(() => {
                onClose()
              }, 1000)
            } else
              ElMessage.error(res.message)
          })
      }).catch((action) => {
        console.log(updateHandle, action);
      })

    const onSubmit = e => {
      const formType = toRaw(props.formType)
      if (formType === 'check') return
      data.formRef.validate((valid) => {
        if (valid) {
          const postData = JSON.stringify(data.formRef.model)
          if (formType === 'create')
            createHandle(postData)
          else if (formType === 'update')
            updateHandle(postData)

        } else {
          console.log('error submit!!')
          return false
        }
      })
    }
    //#endregion

    //#region  methods

    const start = e => {
      data.cfg.checkBtnText = "暂停"
      data.cfg.checkBtnType = "warning"

      const info = toRaw(props.taskInfo)
      task.start({ id: info.taskId }).then(res => {
        data.cfg.checkBtnText = "启动"
        data.cfg.checkBtnType = "primary"
        if (res.ok) {
          ElMessage.success("启动成功")
          data.requestOk = true
          data.resData = res.data
        } else {
          ElMessage.error(res.msg)
        }
      }).catch((e) => {
        data.cfg.checkBtnText = "启动"
        data.cfg.checkBtnType = "primary"
      })
    }

    const dele = e => {
      ElMessageBox.confirm(
        '确定删除?',
        '提示',
        {
          distinguishCancelAndClose: true,
          confirmButtonText: '确定',
          cancelButtonText: '取消',
          type: 'warning',
          center: true,
        }
      ).then(() => {
        task.delete({ id: props.taskInfo.taskId }).then(res => {
          if (res.ok) {
            ElMessage.success("删除成功")
            context.emit('onSuccessed', res)
            setTimeout(() => {
              onClose()
            }, 2000)
          } else {
            ElMessage.error(res.msg)
          }
        })
      }).catch((action) => {
        console.log(updateHandle, action);
      })
    }

    const pause = e => {
      task.pause(data).then(res => {
        if (res.ok) {
          ElMessage.success("启动成功")
        } else {
          ElMessage.error(res.msg)
        }
      })
    }

    const open = args => {
      data.visible = true
    }

    const resolveOpen = e => {
      data.resolverDialogRef.open()
    }

    const validateUrl = (rule, value, callback) => {
      const pattern = new RegExp("(https?|ftp|file)://[-A-Za-z0-9+&@#/%?=~_|!:,.;]+[-A-Za-z0-9+&@#/%=~_|]")
      if (value === '') {
        callback(new Error('请求链接不能为空'))
      } else if (!pattern.test(value)) {
        callback(new Error("输入的不是链接"))
      } else {
        callback()
      }
    }

    // 表单验证规则
    const rules = reactive({
      taskName: {
        required: true,
        message: '任务名不能为空',
        trigger: 'blur',
      },
      url: {
        validator: validateUrl,
        trigger: 'blur',
      },
      method: {
        required: true,
        message: '请求类型必须选择一个',
        trigger: 'change',
      },
      resolveType: {
        required: true,
        message: '解析类型必须选择一个',
        trigger: 'change',
      },
    })

    //#endregion

    return {
      start,
      dele,
      pause,
      open,
      resolveOpen,
      onSubmit,
      onClose,
      rules,
      ...toRefs(data),
    }
  },
})

</script>

<style>
.el-dialog--center .el-dialog__body {
  padding-bottom: 0;
  padding-top: 0;
}
</style>
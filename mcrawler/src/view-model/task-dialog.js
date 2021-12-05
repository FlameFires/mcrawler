import { defineComponent, onBeforeUpdate, onUnmounted, toRaw, isRef, ref, onActivated, onMounted, onUpdated, reactive, toRefs, toRef, watch } from 'vue'
import { ElMessage } from 'element-plus'
import { task } from '../http/http'


const props = {
    formType: {
        type: String,
        default: 'create',
    },
    taskInfo: Object,
}

const data = reactive({
    title: "添加任务",
    visible: false,
    // 表单实例
    formRef: null,
    // resolverDialog 实例
    resolverDialogRef: null,
    // 对话框配置
    cfg: {
        leftButtonEnable: true,
        leftButtonText: '添加',
        rightButtonEnable: true,
        rightButtonText: '关闭',
        checkBtnEnable: false,
        checkBtnType: 'primary',
        checkBtnText: '启动',
        deleteBtnEnable: false,
        resolverBtnEnalbe: false,
        updateEnable: false,
    },
    // 下拉框数据
    selectItems: {
        method: [
            {
                value: 'GET'
            }, {
                value: 'POST'
            }, {
                value: 'DELETE'
            }, {
                value: 'PUT'
            },
        ],
        rType: [
            {
                label: '正则',
                value: 'Regex'
            }, {
                label: 'XPath',
                value: 'XPath'
            }, {
                label: 'JQuery',
                value: 'JQuery'
            }, {
                label: 'Match',
                value: 'Match'
            },
        ]
    },
    // 响应数据
    resData: null,
})




export default {
    data,
    props,
}

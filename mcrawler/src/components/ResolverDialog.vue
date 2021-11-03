<template>
    <el-dialog v-model="visible" :append-to-body="true" title="解析" center>
        <el-form
            ref="formRef"
            :model="taskInfo"
            :status-icon="true"
            label-position="right"
            label-width="80px"
            :rules="rules"
        >
            <el-row>
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

            <el-tabs type="border-card">
                <el-tab-pane label="返回内容">
                    <el-input type="textarea" :rows="15" :readonly="true" v-model="resContent"></el-input>
                </el-tab-pane>
                <el-tab-pane label="预览内容">
                    <!-- <div v-html="resContent"></div> -->
                </el-tab-pane>
                <el-tab-pane label="解析内容">
                    <el-input type="textarea" :rows="15" :readonly="true" v-model="resolveContent"></el-input>
                </el-tab-pane>
            </el-tabs>
        </el-form>

        <template #footer>
            <span class="dialog-footer">
                <!-- 解析 -->
                <el-button type="success" @click="resolve">解析</el-button>

                <!-- 关闭 -->
                <el-button @click="close">关闭</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script>
import { ElMessage } from 'element-plus'
import { ref, toRefs, defineComponent, reactive, toRef, toRaw } from 'vue'

export default defineComponent({
    props: {
        taskInfo: Object,
        resContent: String,
    },
    emits: [],
    setup(props, context) {

        const data = reactive({
            formRef: null,
            visible: false,
            cfg: {

            },
            resolveContent: '',
            selectItems: {
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
            rules: {

            }
        })

        const resolve = () => {
            data.formRef.validate((valid) => {
                if (valid) {
                    const info = toRaw(data.formRef.model)
                    switch (info.resolveType) {
                        case 'Regex':
                            regexResolve(info.resolvePattern)
                            break;
                        case 'XPath':
                            xpathResolve(info.resolvePattern)
                            break;
                        case 'JQuery':
                            jqueryResolve(info.resolvePattern)
                            break;
                        case 'Match':
                            matchResolve(info.resolvePattern)
                            break;
                        default:
                            break;
                    }

                } else {
                    console.log('error submit!!')
                    return false
                }
            })
        }

        //#region resolve methos

        const regexResolve = pattern => {
            const backData = backResolveContent(props.resContent.match(new RegExp(pattern, "ig")))
            data.resolveContent = backData
        }

        const xpathResolve = pattern => {
            let arr = []
            console.log('xpath resolve');

            //#region 这里有问题,用不了,还是通过后台来处理
            let objE = document.createElement("div");
            objE.innerHTML = props.resContent;
            const result = document.evaluate(pattern, objE.childNodes, null, XPathResult.ANY_TYPE, null)
            let node = result.iterateNext()
            while (node) {
                arr.push(node)
                node = result.iterateNext()
            }
            //#endregion

            const backData = backResolveContent(arr)
            data.resolveContent = backData
        }

        const jqueryResolve = pattern => {
            ElMessage.info("未实现此功能")
        }

        const matchResolve = pattern => {
            ElMessage.info("未实现此功能")
        }

        const backResolveContent = resolvedData => {
            let backData = ''
            if (resolvedData instanceof Array) {
                resolvedData.forEach(item => backData += item + "\n")
            }
            else if (resolvedData instanceof String) {
                backData = resolvedData
            }
            else if (resolvedData instanceof Object) {
                backData = resolvedData.toString()
            } else {
                backData = resolvedData
            }
            return backData
        }

        //#endregion



        const open = e => {
            data.visible = true
        }

        const close = e => data.visible = false

        return {
            ...toRefs(data),
            open, close, resolve
        }
    }
})

</script>
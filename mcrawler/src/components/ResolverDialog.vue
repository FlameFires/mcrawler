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
                    <el-input
                        type="textarea"
                        :rows="15"
                        :readonly="true"
                        v-model="responseContent"
                    ></el-input>
                </el-tab-pane>
                <el-tab-pane label="预览内容">
                    <iframe
                        id="previewHtml"
                        ref="previewHtml"
                        type="textarea"
                        frameborder="0"
                        :rows="15"
                        style="
    width: 100%;
    min-height: 350px;
    word-wrap: break-word;
    overflow-y: auto; 
"
                        scrolling="yes"
                        :srcdoc="responseContent"
                    ></iframe>
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

                <!-- 下载 -->
                <el-button type="primary" v-if="cfg.showDownloadBtn" @click="download">下载</el-button>
            </span>
        </template>
    </el-dialog>
</template>

<script lang="ts">
import { ElMessage } from 'element-plus'
import { task } from '../http/http'
import { ref, toRefs, defineComponent, reactive, toRef, toRaw, watchEffect } from 'vue'

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
                showDownloadBtn: false
            },
            responseContent: '',
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
                        label: 'Css',
                        value: 'CssLoad'
                    }, {
                        label: '通用匹配式',
                        value: 'Match'
                    }
                ]
            },
            rules: {
                
            }
        })

        watchEffect(() => {
            if (data.resolveContent)
                data.cfg.showDownloadBtn = data.resolveContent.trim() != '' ? true : false
            if (props.resContent && props.resContent.trim() != '') {
                data.responseContent = props.resContent
            }
        })

        const resolve = () => {
            data.formRef.validate((valid: Boolean) => {
                if (valid && data.responseContent && data.responseContent != 'null') {
                    const info = toRaw(data.formRef.model)
                    switch (info.resolveType) {
                        case 'Regex':
                            regexResolve(info.resolvePattern)
                            break;
                        case 'XPath':
                            xpathResolve(info.resolvePattern)
                            break;
                        case 'CssLoad':
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
                    ElMessage.info('内容为空')
                    return false
                }
            })
        }

        //#region resolve methos

        // 下载
        const download: Function = () => {
            let content = data.resolveContent
            if (!content || content.trim() === '') {
                return;
            }

            let url = window.URL.createObjectURL(new Blob([content]));
            let link = document.createElement("a");
            link.style.display = "none";
            link.href = url;
            link.setAttribute("download", "resolver_file.txt"); //指定下载后的文件名，防跳转
            document.body.appendChild(link);
            link.click();

        }

        // 正则解析
        const regexResolve: Function = (pattern: string) => {
            if (!props.resContent || props.resContent.trim() == '') {
                // 启动任务
                task.start({ id: props.taskInfo.taskId })
                    .then(res => {
                        if (res.ok) {
                            // ElMessage.success("启动成功")
                            data.responseContent = res.data
                            const backData = backResolveContent(res.data.match(new RegExp(pattern, "ig")))
                            data.resolveContent = backData
                        } else {
                            ElMessage.error(res.msg)
                        }
                    }).catch((e) => {
                        console.error(e);
                    })
            } else {
                const backData = backResolveContent(props.resContent.match(new RegExp(pattern, "ig")))
                data.resolveContent = backData
            }
        }

        // xpath解析
        const xpathResolve: Function = (pattern: String) => {

            //#region   启动任务
            // task.resolve({ id: props.taskInfo.taskId })
            task.resolve(props.taskInfo)
                .then(res => {
                    if (res.ok) {
                        console.log("xpath");

                        data.responseContent = res.data.html
                        data.resolveContent = backResolveContent(res.data.result)
                    } else {
                        ElMessage.error(res.msg)
                    }
                }).catch((e: any) => {
                    console.error(e);
                })

            //#endregion

            //#region 这里有问题,用不了,还是通过后台来处理
            // console.log('xpath resolve');
            // let arr = []
            // let objE = document.createElement("div");
            // objE.innerHTML = props.resContent;
            // const result = document.evaluate(pattern, objE.childNodes, null, XPathResult.ANY_TYPE, null)
            // let node = result.iterateNext()
            // while (node) {
            //     arr.push(node)
            //     node = result.iterateNext()
            // }
            // const backData = backResolveContent(arr)
            // data.resolveContent = backData
            //#endregion
        }

        const jqueryResolve: Function = (pattern: String) => {
            ElMessage.error("未实现此功能")
        }

        const matchResolve: Function = (pattern: String) => {
            ElMessage.error("未实现此功能")
        }

        // 解析内容规范化
        const backResolveContent: Function = resolvedData => {
            let backData: String = ''
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


        /**
         * 窗口打开事件
         */
        const open = e => {
            data.visible = true
        }

        /**
         * 窗口关闭事件
         */
        const close = e => {
            data.visible = false
            data.resolveContent = ''
            data.responseContent = ''
        }

        return {
            ...toRefs(data),
            open, close, resolve, download
        }
    }
})

</script>
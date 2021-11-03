import ElementPlus from 'element-plus'
import 'element-plus/dist/index.css'
import router from './router'


/**
 * 插件集
 * @type {import("vue").Plugin[]}
 */
const plugins = [ElementPlus, router]


/**
 * @description: 批量注册插件
 * @param {import("vue").App} app
 * @return {*}
 */
export const usePlugins = app => plugins.forEach(app.use, app)
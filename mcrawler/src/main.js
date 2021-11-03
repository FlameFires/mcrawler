import { createApp } from 'vue'
import { usePlugins } from './plugins/index.js'
// import http from './http/http.js'
import App from './App.vue'

const app = createApp(App)
// app.mixin(http)
usePlugins(app)
app.mount('#app')

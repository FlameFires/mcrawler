import { createRouter, createWebHashHistory } from 'vue-router'

const routes = [
    {
        path: '/',
        redirect: { name: 'home' }
    }, {
        path: '/home',
        name: 'home',
        component: () => import('../pages/home/Index.vue'),
    }, {
        path: '/login',
        name: 'login',
        component: () => import('../pages/login/Index.vue'),
    }, {
        path: '/:catchAll(.*)',
        redirect: { name: 'home' },
    }
]


const router = createRouter({
    // 内部提供了 history 模式的实现。为了简单起见，我们在这里使用 hash 模式。
    history: createWebHashHistory(),
    routes
})


export default router
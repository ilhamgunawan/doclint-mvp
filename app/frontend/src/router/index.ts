import { createRouter, createWebHistory } from 'vue-router'
import LintPage from '../views/LintPage.vue'
import AboutPage from '../views/AboutPage.vue'
import NotFoundPage from '../views/NotFoundPage.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', redirect: '/lint' },
    { path: '/lint', name: 'lint', component: LintPage },
    { path: '/about', name: 'about', component: AboutPage },
    { path: '/:pathMatch(.*)*', name: 'not-found', component: NotFoundPage },
  ],
})

export default router

<template>
    <el-aside style="height: 100%; width: auto; background: white">
        <el-menu
            default-active="2"
            class="el-menu-vertical-demo"
            :collapse="isCollapse"
            @open="handleOpen"
            @close="handleClose"
        >
            <el-menu-item index="0" @click="switchCollapse">
                <i class="el-icon-menu"></i>
                <template #title>Collapse Zero</template>
            </el-menu-item>
            <el-sub-menu index="1">
                <template #title>
                    <i class="el-icon-location"></i>
                    <span>Navigator One</span>
                </template>
                <el-menu-item-group>
                    <template #title>
                        <span>Group One</span>
                    </template>
                    <el-menu-item index="1-1">item one</el-menu-item>
                    <el-menu-item index="1-2">item two</el-menu-item>
                </el-menu-item-group>
                <el-menu-item-group title="Group Two">
                    <el-menu-item index="1-3">item three</el-menu-item>
                </el-menu-item-group>
                <el-sub-menu index="1-4">
                    <template #title>
                        <span>item four</span>
                    </template>
                    <el-menu-item index="1-4-1">item one</el-menu-item>
                </el-sub-menu>
            </el-sub-menu>

            <el-menu-item index="2">
                <i class="el-icon-menu"></i>
                <template #title>Navigator Two</template>
            </el-menu-item>
            <el-menu-item index="3" disabled>
                <i class="el-icon-document"></i>
                <template #title>Navigator Three</template>
            </el-menu-item>
            <el-menu-item index="4">
                <i class="el-icon-setting"></i>
                <template #title>Navigator Four</template>
            </el-menu-item>
        </el-menu>
    </el-aside>
</template>


<script lang="ts">
import { defineComponent, ref, reactive, toRefs } from 'vue';

export default defineComponent({
    data() {
        return {
            isCollapse: false
        }
    },
    methods: {
    },
    setup() {

        let isCollapse = ref(false);
        const handleOpen = (key: string, keyPath: string) => {
            console.log(key, keyPath);
        };
        const handleClose = (key: string, keyPath: string) => {
            console.log(key, keyPath);
        };

        const switchCollapse = () => {
            isCollapse.value = !isCollapse.value;
        }
        return { isCollapse, handleOpen, handleClose, switchCollapse }
    }
});
</script>
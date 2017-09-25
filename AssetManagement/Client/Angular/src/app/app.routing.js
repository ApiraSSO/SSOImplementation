"use strict";
Object.defineProperty(exports, "__esModule", { value: true });
var login_component_1 = require("./login/login.component");
exports.AppRoutes = [
    {
        path: '',
        redirectTo: '/login',
        pathMatch: 'full'
    },
    { path: 'login', component: login_component_1.LoginComponent, data: { title: 'Login' } },
    { path: '**', component: login_component_1.LoginComponent, data: { title: 'Login' } },
];
//# sourceMappingURL=app.routing.js.map
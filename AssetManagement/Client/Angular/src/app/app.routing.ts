import { LoginComponent } from './login/login.component';
export const AppRoutes = [
	{
		path: '',
		redirectTo: '/login',
		pathMatch: 'full'
	},
	{ path: 'login', component: LoginComponent, data: { title: 'Login' } },
	{ path: '**', component: LoginComponent, data: { title: 'Login' } },
];
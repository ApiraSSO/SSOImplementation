import { NgModule }      from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';
import { LoginComponent } from './login/login.component';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { AppComponent } from './app.component';
import { AppRoutes } from './app.routing';
import { RouterModule, Router } from '@angular/router';

@NgModule({
	imports:
	[
		BrowserModule,
		FormsModule,
		RouterModule.forRoot(AppRoutes),
	],
	declarations: [AppComponent, LoginComponent ],
	bootstrap: [AppComponent ]
})
export class AppModule { }

import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { FormsModule } from '@angular/forms';

//import { UserService } from '../../services/user.service';

@Component({
    moduleId: module.id,
    selector: 'login',
    templateUrl: 'login.component.html',
    styleUrls: ['login.component.scss']
})
export class LoginComponent {

    public showWarningMessage = false;
    public showErrorMessage = false;

    email:string;
    password: string;

    constructor(
        private router: Router
		//private userService: UserService
	)
	{

    }


    onSubmit(): void {

        this.clearMessages();

        //this.userService.logIn(this.email, this.password);

        //this.userService.currentUser$
        //    .subscribe(user => {
        //        if (user != null) {
        //            this.clearMessages();
        //            this.router.navigate(['/']);   
        //        }
        //        else {
        //            this.showErrorMessage = false;
        //            this.showWarningMessage = true;
        //        }                    
        //    },
        //    error => {
        //        this.showErrorMessage = true;
        //        this.showWarningMessage = false;
        //    });
    }

    private clearMessages(): void {
        this.showWarningMessage = false;
        this.showErrorMessage = false;
    }
}
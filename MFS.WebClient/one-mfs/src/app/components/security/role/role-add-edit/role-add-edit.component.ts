import { Component, OnInit, ViewEncapsulation } from '@angular/core';
import { RoleService } from '../../../../services/security';
import { first } from 'rxjs/operators';
import { Router, ActivatedRoute } from '@angular/router';

@Component({
  selector: 'app-role-add-edit',
  templateUrl: './role-add-edit.component.html',
  styleUrls: ['./role-add-edit.component.css']
})
export class RoleAddEditComponent implements OnInit {

    entityId: number;
    isEditMode: boolean = false;

    roleModel: any = {};

    constructor(private roleService: RoleService, private router: Router, private route: ActivatedRoute) { }

    ngOnInit() {
        this.entityId = +this.route.snapshot.paramMap.get('id');
        if (this.entityId) {
            this.isEditMode = true;
            this.getRoleById();
        }
    }
    getRoleById(): any {
        this.roleService.getRoleById(this.entityId).pipe(first())
            .subscribe(
                data => {
                    this.roleModel = data;
                },
                error => {
                    console.log(error);
                });

    };


    onRoleSave() {
        if (this.roleModel.Name != "") {
            this.roleService.save(this.roleModel).pipe(first())
                .subscribe(
                    data => {
                        console.log(data);
                        this.router.navigateByUrl('./');
                    },
                    error => {
                        console.log(error);
                    });

        }
    }

    onDeleteRole(event) {
        if (this.roleModel && this.roleModel.id) {
            this.roleService.delete(this.roleModel).pipe(first())
                .subscribe(
                    data => {
                        console.log(data);
                        this.router.navigateByUrl('../feature-category/worklist');
                    },
                    error => {
                        console.log(error);
                    });
        }
    }

}

import { UsersFilter } from 'src/app/_models/filters/users-filter';
import { AppointmentService } from 'src/app/_services/appointment.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { DatePipe } from '@angular/common';
import { Appointment } from 'src/app/_models/appointment';
import Swal from 'sweetalert2';
import { User } from '../_models/user';

@Component({ templateUrl: 'users.component.html' })
export class UsersComponent implements OnInit {
    formFilter!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private activatedRoute: ActivatedRoute,
        private accountService: AccountService,
        private alertService: AlertService,
        private dp: DatePipe
    ) { 
        // redirect to home if already logged in
        if (this.accountService.userValue.role !== 'administrator') {
            this.router.navigate(['/']);
        }

        this.formFilter = this.formBuilder.group({
            fullName: ['', [Validators.maxLength(100)]],
            belongsToRiskGroup: [false],
            isActive: [true],
        });
        

            this.route.queryParams.subscribe((params) => {
                if (params.type) {
                    this.filter.role = params.type;
                }
                if (params.fullName) {
                    this.formFilter.controls.fullName.setValue(params.fullName, {
                        onlySelf: true,
                      });
                }
                if (params.isActive) {
                  this.formFilter.controls.isActive.setValue(params.isActive === "true", {
                    onlySelf: true,
                  });
                }
          
                if (params.belongsToRiskGroup) {
                  this.formFilter.controls.belongsToRiskGroup.setValue(params.belongsToRiskGroup === "true", {
                    onlySelf: true,
                  });
                }
                this.loadData();
              });
    }

    public users: User[] = [];
    public filter = new UsersFilter();

    ngOnInit() {
       
    }

    loadData() {
        const fullName = this.formFilter.get('fullName')?.value;
        const isActive = this.formFilter.get('isActive')?.value;
        const belongsToRiskGroup = this.formFilter.get('belongsToRiskGroup')?.value;
        this.filter.fullName =fullName;
        this.filter.isActive =isActive;
        this.filter.belongsToRiskGroup=belongsToRiskGroup;
        console.log(this.filter);
        this.accountService.getAll(this.filter).subscribe((res: any) => {
            this.users = res.users;
        });

    }

    panelOpenState = false;

    // convenience getter for easy access to form fields
    get f() { return this.formFilter.controls; }

    applyFilter() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.formFilter.invalid) {
            return;
        }

        this.loading = true;

        const fullName = this.formFilter.get('fullName')?.value;
        const isActive = this.formFilter.get('isActive')?.value;
        const belongsToRiskGroup = this.formFilter.get('belongsToRiskGroup')?.value;
        const queryParams: any = {};

        queryParams.type = this.route.snapshot.queryParamMap.get('type');
        //const filter = this.route.snapshot.queryParamMap.get('filter');

        console.log(this.route.snapshot.queryParamMap.get('type'));

          if (fullName) {
            queryParams.fullName = fullName;
          }
          if (isActive !== undefined) {
            queryParams.isActive = isActive;
          }
          if (belongsToRiskGroup !== undefined) {
            queryParams.belongsToRiskGroup = belongsToRiskGroup;
          }

          this.router.navigate(['/users'], {
            queryParams,
          });

          this.loading = false;
    }

    clear() {
      // reset alerts on submit
      this.alertService.clear();

      this.formFilter.controls.fullName.setValue('', {
        onlySelf: true,
      });
      this.formFilter.controls.isActive.setValue(true);
      this.formFilter.controls.belongsToRiskGroup.setValue(false);

      this.applyFilter();
  }
}
import { OfficeService } from 'src/app/_services/office.service';
import { Component, OnInit } from '@angular/core';
import { Router, ActivatedRoute } from '@angular/router';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { first } from 'rxjs/operators';
import { AccountService } from 'src/app/_services/account.service';
import { AlertService } from 'src/app/_services/alert.service';
import { DatePipe } from '@angular/common';
import Swal from 'sweetalert2';
import { Office } from '../_models/office';
import { OfficesFilter } from '../_models/filters/offices-filter';

@Component({ templateUrl: 'offices.component.html' })
export class OfficesComponent implements OnInit {
    formFilter!: FormGroup;
    loading = false;
    submitted = false;

    constructor(
        private formBuilder: FormBuilder,
        private route: ActivatedRoute,
        private router: Router,
        private accountService: AccountService,
        private officeService: OfficeService,
        private alertService: AlertService
    ) { 
        // redirect to home if already logged in
        if (this.accountService.userValue.role !== 'administrator') {
            this.router.navigate(['/']);
        }

        this.formFilter = this.formBuilder.group({
            name: ['', [Validators.maxLength(100)]],
            isActive: [true],
        });
        

            this.route.queryParams.subscribe((params) => {
             
                if (params.name) {
                    this.formFilter.controls.name.setValue(params.name, {
                        onlySelf: true,
                      });
                }
                if (params.isActive) {
                  this.formFilter.controls.isActive.setValue(params.isActive === "true", {
                    onlySelf: true,
                  });
                }
                this.loadData();
              });
    }

    public offices: Office[] = [];
    public filter = new OfficesFilter();

    ngOnInit() {
       
    }

    loadData() {
        const name = this.formFilter.get('name')?.value;
        const isActive = this.formFilter.get('isActive')?.value;
        this.filter.name =name;
        this.filter.isActive =isActive;
        this.officeService.getAll(this.filter).subscribe((res: any) => {
            this.offices = res.offices;
        });
    }

    panelOpenState = false;

    // convenience getter for easy access to form fields
    get f() { return this.formFilter.controls; }

    deleteOfficeQuestion(o: Office) {
      Swal
      .fire({
        title: '¿Está seguro?',
        text: 'Dar de baja a ' + o.name,
        icon: 'warning',
        showCancelButton: true,
        cancelButtonText: 'No, cancelar',
        confirmButtonText: 'Si, dar de baja!'
      })
      .then(result => {
        if (result.value) {
          this.deleteOffice(o);
          
        }
      });
    }

    deleteOffice(o: Office) {
      this.officeService.canBeDeleted(+o.id).subscribe((res: boolean) => {
        if (!res) {
          Swal
      .fire({
        title: 'Oops...',
        text: 'No se puede dar de baja ya que posee turnos pendientes y/o confirmados. Por favor, cancélelos primero.',
        icon: 'error',
      })
        } else {
          this.doDelete(o);
        }
    });
  }

  doDelete(o: Office) {
    this.loading = true;
    this.officeService.cancel(+o.id).pipe(first())
      .subscribe({
          next: () => {
            Swal
            .fire({
              title: 'Hecho',
              text: 'Sede desactivada correctamente.',
              icon: 'success',
            });
                this.loadData();
                this.loading = false;
          },
          error: (error: string) => {
              this.alertService.error(error);
              this.loading = false;
          }
      });
  }

    editOffice(o: Office) {
        this.router.navigate(['offices/edit/', o.id]);
    }

    applyFilter() {
        this.submitted = true;

        // reset alerts on submit
        this.alertService.clear();

        // stop here if form is invalid
        if (this.formFilter.invalid) {
            return;
        }

        this.loading = true;

        const name = this.formFilter.get('name')?.value;
        const isActive = this.formFilter.get('isActive')?.value;
        const queryParams: any = {};

          if (name) {
            queryParams.name = name;
          }
          if (isActive !== undefined) {
            queryParams.isActive = isActive;
          }

          this.router.navigate(['/offices'], {
            queryParams,
          });

          this.loading = false;
    }

    clear() {
      // reset alerts on submit
      this.alertService.clear();

      this.formFilter.controls.name.setValue('', {
        onlySelf: true,
      });
      this.formFilter.controls.isActive.setValue(true);

      this.applyFilter();
  }

  addOffice() {
    this.router.navigate(['offices','new']);
  }
}
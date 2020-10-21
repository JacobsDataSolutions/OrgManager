import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'org-manager-add-or-update-customer',
  templateUrl: './add-or-update-customer.component.html',
  styleUrls: ['./add-or-update-customer.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AddOrUpdateCustomerComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}

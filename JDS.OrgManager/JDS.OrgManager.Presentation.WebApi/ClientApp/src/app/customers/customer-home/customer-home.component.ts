import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'om-customer-home',
  templateUrl: './customer-home.component.html',
  styleUrls: ['./customer-home.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class CustomerHomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}

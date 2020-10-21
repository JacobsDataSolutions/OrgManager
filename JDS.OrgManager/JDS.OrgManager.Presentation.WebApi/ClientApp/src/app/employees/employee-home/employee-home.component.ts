import { Component, OnInit, ChangeDetectionStrategy } from '@angular/core';

@Component({
  selector: 'org-manager-employee-home',
  templateUrl: './employee-home.component.html',
  styleUrls: ['./employee-home.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EmployeeHomeComponent implements OnInit {

  constructor() { }

  ngOnInit(): void {
  }

}

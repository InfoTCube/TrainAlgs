import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-dropdown',
  templateUrl: './dropdown.component.html',
  styleUrls: ['./dropdown.component.scss']
})
export class DropdownComponent implements OnInit {
  @Input() Title: string = "Dropdown";
  @Input() Items: string[] = ['Item 1', 'Item 2', 'Item 3', 'Item 4'];
  @Output() OnChange: EventEmitter<string> = new EventEmitter<string>();
  show: boolean = false;

  constructor() { }

  ngOnInit(): void {
  }

  toggle() {
    this.show = !this.show;
  }

  select(item: string) {
    this.OnChange.emit(item);
    this.toggle();
  }
}

import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-lang-dropdown',
  templateUrl: './lang-dropdown.component.html',
  styleUrls: ['./lang-dropdown.component.scss']
})
export class LangDropdownComponent implements OnInit {
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

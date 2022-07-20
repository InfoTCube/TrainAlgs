import { AfterViewChecked, AfterViewInit, Component, ElementRef, EventEmitter, Input, OnChanges, Output, SimpleChanges, ViewChild } from '@angular/core';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import * as Prism from 'prismjs';

@Component({
  selector: 'app-code-editor',
  templateUrl: './code-editor.component.html',
  styleUrls: ['./code-editor.component.scss']
})
export class CodeEditorComponent implements AfterViewInit, OnChanges{
  @Output() getCodeEvent = new EventEmitter<string>();
  @ViewChild('codeEle') codeEle!: ElementRef;
  @ViewChild('numbers') numbers!: ElementRef;
  @Input() lang?: string = 'cpp';
  lineNums = Array.from({length: 20}, (_, i) => i + 1);

  constructor() { }

  ngOnChanges(): void {
    setTimeout(() => {
      Prism.highlightElement(this.codeEle.nativeElement)
    }, 0);
  }


  ngAfterViewInit() {
    Prism.highlightElement(this.codeEle.nativeElement);
  }

  highlight($event) {
    var restore = null;
    switch($event.data) {
      case '(':
        document.execCommand('insertHTML', false, ')');
        restore = this.cursorBackByOneIndex();
        break;
      case '[':
        document.execCommand('insertHTML', false, ']');
        restore = this.cursorBackByOneIndex();
        break;
      case '{':
        document.execCommand('insertHTML', false, '}');
        restore = this.cursorBackByOneIndex();
        break;
      case '"':
        document.execCommand('insertHTML', false, '"');
        restore = this.cursorBackByOneIndex();
        break;
      case '\'':
        document.execCommand('insertHTML', false, '\'');
        restore = this.cursorBackByOneIndex();
        break;
    }
    if(restore == null) restore = this.saveCaretPosition(this.codeEle.nativeElement);
    Prism.highlightElement(this.codeEle.nativeElement);
    restore();
    this.getCode();
  }

  saveCaretPosition(context){
    var selection = window.getSelection();
    var range = selection.getRangeAt(0);
    range.setStart( context, 0 );
    var len = range.toString().length;

    return () => {
      var pos = this.getTextNodeAtPosition(context, len);
      selection.removeAllRanges();
      var range = new Range();
      range.setStart(pos.node ,pos.position);
      selection.addRange(range);
    }
  }

  cursorBackByOneIndex() {
    var context = this.codeEle.nativeElement;
    var selection = window.getSelection();
    var range = selection.getRangeAt(0);
    range.setStart( context, 0 );
    var len = range.toString().length;

    return () => {
      var pos = this.getTextNodeAtPosition(context, len);
      selection.removeAllRanges();
      var range = new Range();
      range.setStart(pos.node ,pos.position-1);
      selection.addRange(range);
    }
  }

  getTextNodeAtPosition(root, index){
    const NODE_TYPE = NodeFilter.SHOW_TEXT;
    var treeWalker = document.createTreeWalker(root, NODE_TYPE, function next(elem) {
      if(index > elem.textContent.length){
        index -= elem.textContent.length;
        return NodeFilter.FILTER_REJECT
      }
      return NodeFilter.FILTER_ACCEPT;
    });
    var c = treeWalker.nextNode();
    return {
      node: c? c: root,
      position: index
    };
  }

  checkForTab($event) {
    if($event.key == 'Tab') {
      $event.preventDefault();
      document.execCommand('insertHTML', false, '&#009');
    }
  }

  scroll($event) {
    console.log(this.lineNums.length)
    this.numbers.nativeElement.scrollTop = this.codeEle.nativeElement.scrollTop;
    if(this.lineNums.length < 13+Math.ceil(this.numbers.nativeElement.scrollTop/23.28))
      this.lineNums = Array.from({length: 13+Math.ceil(this.numbers.nativeElement.scrollTop/23.28)}, (_, i) => i + 1);
  }

  getCode() {
    this.getCodeEvent.emit(this.codeEle.nativeElement.innerText);
  }
}

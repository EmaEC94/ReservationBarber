import { HttpClient } from '@angular/common/http';
import { Component, OnInit } from '@angular/core';
import { CdkDragDrop, moveItemInArray, CdkDropList, CdkDrag, CdkDragHandle, CdkDragPlaceholder } from '@angular/cdk/drag-drop';
import { UntypedFormGroup, UntypedFormControl, UntypedFormBuilder, FormsModule, ReactiveFormsModule } from '@angular/forms';
import { MatSidenav, MatSidenavModule } from '@angular/material/sidenav';
import { LandingPage } from '../landing-page.model';
import { NgClass, DatePipe } from '@angular/common';
import { NgScrollbar } from 'ngx-scrollbar';
import { MatDatepickerModule } from '@angular/material/datepicker';
import { MatOptionModule } from '@angular/material/core';
import { MatSelectModule } from '@angular/material/select';
import { MatCheckboxModule } from '@angular/material/checkbox';
import { MatInputModule } from '@angular/material/input';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatIconModule } from '@angular/material/icon';
import { MatTooltipModule } from '@angular/material/tooltip';
import { MatButtonModule } from '@angular/material/button';
import { MatCardModule } from '@angular/material/card';
import { MatDividerModule } from '@angular/material/divider';
import { Router, RouterLink } from '@angular/router';

@Component({
    selector: 'app-landing-page',
    templateUrl: './landing-page.component.html',
    styleUrls: ['./landing-page.component.scss'],
    imports: [
        MatDividerModule,
        MatCardModule,
        MatButtonModule,
        MatSidenavModule,
        MatTooltipModule,
        MatIconModule,
        FormsModule,
        ReactiveFormsModule,
        MatFormFieldModule,
        MatInputModule,
        MatCheckboxModule,
        MatSelectModule,
        MatOptionModule,
        MatDatepickerModule
    ]
})
export class LandingPageComponent implements OnInit {

  taskForm: UntypedFormGroup;
  showFiller = false;
  isNewEvent = false;
  dialogTitle?: string;
  userImg?: string;
  tasks: LandingPage[] = [];

   items = document.querySelectorAll('.slider .list .item');
   prevBtn = document.getElementById('prev');
   nextBtn = document.getElementById('next');
   lastPosition = this.items.length - 1;
   firstPosition = 0;
   active = 0;


  breadscrums = [
    {
      title: 'Tasks',
      items: ['Home'],
      active: 'Tasks',
    },
  ];

  constructor(private fb: UntypedFormBuilder, private http: HttpClient,  private router: Router,) {
    const blank = {} as LandingPage;
    this.taskForm = this.createFormGroup(blank);

    this.fetch((data: LandingPage[]) => {
      this.tasks = data;
    });
  }
  ngOnInit(): void {
    this.setDiameter();

  }


  nextBtnSlider(){
    this.active = this.active + 1;
    this.setSlider();
  }


  setSlider() {
    let oldActive = document.querySelector('.slider .list .item.active');
    if(oldActive) oldActive.classList.remove('active');
    this.items[this.active].classList.add('active');
    // 
    this.nextBtn!.classList.remove('d-none');
    this.prevBtn!.classList.remove('d-none');

    if(this.active == this.lastPosition) this.nextBtn!.classList.add('d-none');
    if(this.active == this.firstPosition) this.prevBtn!.classList.add('d-none');
}



 setDiameter (){
  let slider = document.querySelector('.slider');
  let widthSlider = 1200;
  let heightSlider = 1200;
  let diameter = Math.sqrt(Math.pow(widthSlider, 2) + Math.pow(heightSlider, 2));
  document.documentElement.style.setProperty('--diameter', diameter+'px');
}
 
agendarCorte(){
  console.log("EMa")
  this.router.navigate(['/calendar'])

}



  fetch(cb: (i: LandingPage[]) => void) {
    const req = new XMLHttpRequest();
    req.open('GET', 'assets/data/task.json');
    req.onload = () => {
      const data = JSON.parse(req.response);
      cb(data);
    };
    req.send();
  }

  drop(event: CdkDragDrop<string[]>) {
    moveItemInArray(this.tasks, event.previousIndex, event.currentIndex);
  }
  toggle(task: { done: boolean }, nav: MatSidenav) {
    nav.close();
    task.done = !task.done;
  }
  addNewTask(nav: MatSidenav) {
    this.resetFormField();
    this.isNewEvent = true;
    this.dialogTitle = 'New Task';
    this.userImg = 'assets/images/user/user1.jpg';
    nav.open();
  }
  taskClick(task: LandingPage, nav: MatSidenav): void {
    this.isNewEvent = false;
    this.dialogTitle = 'Edit Task';
    this.userImg = task.img;
    nav.open();
    this.taskForm = this.createFormGroup(task);
  }
  closeSlider(nav: MatSidenav) {
    nav.close();
  }
  createFormGroup(data: LandingPage) {
    return this.fb.group({
      id: [data ? data.id : this.getRandomID()],
      img: [data ? data.img : 'assets/images/user/user1.jpg'],
      name: [data ? data.name : null],
      title: [data ? data.title : null],
      done: [data ? data.done : null],
      priority: [data ? data.priority : null],
      due_date: [data ? data.due_date : null],
      note: [data ? data.note : null],
    });
  }

  saveTask() {
    this.tasks.unshift(this.taskForm.value);
    this.resetFormField();
  }

  editTask() {
    const targetIdx = this.tasks
      .map((item) => item.id)
      .indexOf(this.taskForm.value.id);
    this.tasks[targetIdx] = this.taskForm.value;
  }
  deleteTask(nav: MatSidenav) {
    const targetIdx = this.tasks
      .map((item) => item.id)
      .indexOf(this.taskForm.value.id);
    this.tasks.splice(targetIdx, 1);
    nav.close();
  }

  resetFormField() {
    this.taskForm.controls['name'].reset();
    this.taskForm.controls['title'].reset();
    this.taskForm.controls['done'].reset();
    this.taskForm.controls['priority'].reset();
    this.taskForm.controls['due_date'].reset();
    this.taskForm.controls['note'].reset();
  }

  public getRandomID(): number {
    const S4 = () => {
      return ((1 + Math.random()) * 0x10000) | 0;
    };
    return S4() + S4();
  }

}



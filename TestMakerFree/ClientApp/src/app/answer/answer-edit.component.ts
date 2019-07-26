import { Component, Inject, OnInit } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { error } from "protractor";

@Component({
  selector: "answer-edit",
  templateUrl: "./answer-edit.component.html",
  styleUrls: ["./answer-edit.component.css"]
})

export class AnswerEditComponent {
  title: string;
  answer: Answer;
  //this item will be TRUE when editing an existing answer, FALSE when creating a new one
  editMode: boolean;
  form: FormGroup;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private fb: FormBuilder,
    @Inject('BASE_URL') private base_url: string) {
    //create an empty object from the Question interface
    this.answer = <Answer>{};

    //initialize the form
    this.createForm();

    //a plus before a variable returns its number representation!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    var id = +this.activatedRoute.snapshot.params["id"];

    //check if we're in edit-mode or not
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //fetch the answer from the server: in this case the id is the answerId
      var url = this.base_url + "api/answer/" + id;
      this.http.get<Answer>(url).subscribe(res => {
        this.answer = res;
        this.title = "Edit - " + this.answer.Text;

        //update the form controls
        this.updateForm();
      }, error => console.log(error))
    }
    else {
      //create new answer for the Question: in this case the id is the questionId
      this.answer.QuestionId = id;
      this.title = "Create a new Answer";
    }
  }

  onSubmit() {
    //create a temporary answer object
    var tempAnswer = <Answer>{};
    tempAnswer.Text = this.form.value.Text;
    tempAnswer.Value = this.form.value.Value;
    tempAnswer.QuestionId = this.answer.QuestionId;

    var url = this.base_url + "api/answer";
    if (this.editMode) {
      tempAnswer.Id = this.answer.Id;

      this.http.post<Answer>(url, tempAnswer).subscribe(res => {
        var v = res;
        console.log("Answer " + v.Id + " has been updated.");
        this.router.navigate(["question/edit", v.QuestionId]);
      }, error => console.log(error));
    }
    else {
      this.http.put<Answer>(url, tempAnswer).subscribe(res => {
        var v = res;
        console.log("Answer " + v.Id + " has been created");
        this.router.navigate(["question/edit", v.QuestionId]);
      }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["question/edit", this.answer.QuestionId]);
  }

  createForm() {
    this.form = this.fb.group({
      Text: ["", Validators.required],
      Value: ["", [Validators.required, Validators.min(-5), Validators.max(5)]]
    });
  }

  updateForm() {
    this.form = this.fb.group({
      Text: this.answer.Text || "",
      Value: this.answer.Value || 0
    });
  }

  //retrieve a FormControl
  getFormControl(name: string) {
    return this.form.get(name);
  }

  //returns TRUE if the FormControl is valid
  isValid(name: string) {
    var e = this.getFormControl(name);
    return e && e.valid;
  }

  //return TRURE if the FormControl has been changed
  isChanged(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched);
  }

  //returns TRUE if the FormControl is invalid after user changes
  hasError(name: string) {
    var e = this.getFormControl(name);
    return e && (e.dirty || e.touched) && !e.valid;
  }

}

import { Component, Inject, OnInit } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms"
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { error } from "protractor";

@Component({
  selector: "question-edit",
  templateUrl: "./question-edit.component.html",
  styleUrls: ["./question-edit.component.css"]
})

export class QuestionEditComponent {
  title: string;
  question: Question;
  //this item will be TRUE when editing an existing question, FALSE when creating a new one
  editMode: boolean;
  form: FormGroup;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private fb: FormBuilder,
    @Inject('BASE_URL') private base_url: string) {
    //create an empty object from the Quiz interface
    this.question = <Question>{};

    //initialize the form
    this.createForm();

    //a plus before a variable returns its number representation!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    var id = +this.activatedRoute.snapshot.params["id"];

    //check if we're in edit-mode or not
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //fetch the question from the server: in this case the id is the questionId
      var url = this.base_url + "api/question/" + id;
      this.http.get<Question>(url).subscribe(res => {
        this.question = res;
        this.title = "Edit - " + this.question.Text;

        //update the from with the question value
        this.updateForm();
      }, error => console.log(error))
    }
    else {
      //create new question for the Quiz: in this case the id is the quizId
      this.question.QuizId = id;
      this.title = "Create a new Question";
    }
  }

  onSubmit() {
    //build a temporary question value from the form fields
    var tempQuestion = <Question>{};
    tempQuestion.Text = this.form.value.Text;

    var url = this.base_url + "api/question";
    if (this.editMode) {
      //don't forget to set the question id and quizid, otherwise the EDIT would fail!
      tempQuestion.Id = this.question.Id;
      tempQuestion.QuizId = this.question.QuizId;

      this.http.post<Question>(url, tempQuestion).subscribe(res => {
        var v = res;
        console.log("Question " + v.Id + " has been updated.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.log(error));
    }
    else {
      this.http.put<Question>(url, tempQuestion).subscribe(res => {
        var v = res;
        console.log("Question " + v.Id + " has been created");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.question.QuizId]);
  }

  createForm() {
    this.form = this.fb.group({
      Text: ["", Validators.required]
    });
  }

  updateForm() {
    this.form = this.fb.group({
      Text: this.question.Text || ""
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

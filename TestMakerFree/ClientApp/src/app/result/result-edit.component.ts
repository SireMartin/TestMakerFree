import { Component, Inject, OnInit } from "@angular/core";
import { FormGroup, FormControl, FormBuilder, Validators } from "@angular/forms";
import { HttpClient } from "@angular/common/http";
import { Router, ActivatedRoute } from "@angular/router";
import { error } from "protractor";

@Component({
  selector: "result-edit",
  templateUrl: "./result-edit.component.html",
  styleUrls: ["./result-edit.component.css"]
})

export class ResultEditComponent {
  title: string;
  result: Result;
  //this item will be TRUE when editing an existing result, FALSE when creating a new one
  editMode: boolean;
  form: FormGroup;

  constructor(private activatedRoute: ActivatedRoute,
    private router: Router,
    private http: HttpClient,
    private fb: FormBuilder,
    @Inject('BASE_URL') private base_url: string) {
    //create an empty object from the Quiz interface
    this.result = <Result>{};

    //initialize the form
    this.createForm();

    //a plus before a variable returns its number representation!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    var id = +this.activatedRoute.snapshot.params["id"];

    //check if we're in edit-mode or not
    this.editMode = (this.activatedRoute.snapshot.url[1].path === "edit");
    if (this.editMode) {
      //fetch the result from the server: in this case the id is the resultId
      var url = this.base_url + "api/result/" + id;
      this.http.get<Result>(url).subscribe(res => {
        this.result = res;
        this.title = "Edit - " + this.result.Text;

        //update the form controls
        this.updateForm();
      }, error => console.log(error))
    }
    else {
      //create new result for the Quiz: in this case the id is the quizId
      this.result.QuizId = id;
      this.title = "Create a new Result";
    }
  }

  onSubmit() {
    //create a temporary result object
    var tempResult = <Result>{};
    tempResult.Text = this.form.value.Text;
    tempResult.MinValue = this.form.value.MinValue;
    tempResult.MaxValue = this.form.value.MaxValue;
    tempResult.QuizId = this.result.QuizId;

    var url = this.base_url + "api/result";
    if (this.editMode) {
      tempResult.Id = this.result.Id;
      this.http.post<Result>(url, tempResult).subscribe(res => {
        var v = res;
        console.log("Result " + v.Id + " has been updated.");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.log(error));
    }
    else {
      this.http.put<Result>(url, tempResult).subscribe(res => {
        var v = res;
        console.log("Result " + v.Id + " has been created");
        this.router.navigate(["quiz/edit", v.QuizId]);
      }, error => console.log(error));
    }
  }

  onBack() {
    this.router.navigate(["quiz/edit", this.result.QuizId]);
  }

  createForm() {
    this.form = this.fb.group({
      Text: ["", Validators.required],
      MinValue: ["", Validators.pattern(/^\d*$/)],
      MaxValue: ["", Validators.pattern(/^\d*$/)]
    });
  }

  updateForm() {
    this.form = this.fb.group({
      Text: this.result.Text,
      MinValue: this.result.MinValue || "",
      MaxValue: this.result.MaxValue || ""
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

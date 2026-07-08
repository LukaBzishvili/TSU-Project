import { Component } from '@angular/core';
// import { BlogCardProps } from '../../models/models';
import { CommonModule } from '@angular/common';
import { BlogCard } from '../blog-card/blog-card';

@Component({
  selector: 'app-main-blogs',
  imports: [CommonModule, BlogCard],
  templateUrl: './main-blogs.html',
  styleUrl: './main-blogs.css',
})
export class MainBlogs {
  // blogs: BlogCardProps[] = [];
  slides = ['Slide 1', 'Slide 2', 'Slide 3', 'Slide 4'];
}

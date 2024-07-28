## 2024.0728.2
* "<mm:dd.ss>" in the `.lrc` file is absolute time, not the relative time.

## 2024.0728.1
* Add `StartTime` property in the `Model.Lyric` class. This property is used to store the start time of the lyric.~~
* `LrcParse` is re-written. Now it can follow the [LRC and Enhanced LRC format](https://en.wikipedia.org/wiki/LRC_(file_format) to decode/encode the lyric correctly.
* Create the `KarParser`, which is used to parse the Karaoke file with format like `[00:51.00]ka[01:29.99]ra[01:48.29]o[02:31.00]ke[02:41.99]`.
